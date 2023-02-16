using System;
using System.Collections.Generic;
using System.Linq;

class Uint8WriteStream {
    public byte[] buffer = null;
    public int index = 0;

    int length = 0;
    int extendedSize = 0;

    public Uint8WriteStream(int extendedSize) {
        buffer = new byte[extendedSize];
        length = extendedSize;
        this.extendedSize = extendedSize;
    }

    public void write(int value) {
        if (length <= index) {
            length += extendedSize;
            var newBuffer = new byte[length];
            var nowSize = buffer.Length;
            for (int i = 0; i < nowSize; i++) {
                newBuffer[i] = buffer[i];
            }
            buffer = newBuffer;
        }
        buffer[index] = (byte)value;
        index++;
    }
}

class BitReadStream {
    public byte[] buffer = null;
    public int bufferIndex = 0;
    public int nowBits = 0;
    public int nowBitsLength = 8;
    public bool isEnd = false;

    public BitReadStream(byte[] buffer, int offset = 0) {
        this.buffer = buffer;
        bufferIndex = offset;
        nowBits = buffer[offset];
    }

    public int read() {
        if (isEnd) {
            throw new Exception("Lack of data length");
        }
        var bit = nowBits & 1;
        if (nowBitsLength > 1) {
            nowBitsLength--;
            nowBits >>= 1;
        } else {
            bufferIndex++;
            if (bufferIndex < buffer.Length) {
                nowBits = buffer[bufferIndex];
                nowBitsLength = 8;
            } else {
                nowBitsLength = 0;
                isEnd = true;
            }
        }
        return bit;
    }

    public int readRange(int length) {
        while (nowBitsLength <= length) {
            nowBits |= buffer[++bufferIndex] << nowBitsLength;
            nowBitsLength += 8;
        }
        var bits = nowBits & ((1 << length) - 1);
        nowBits >>= length;
        nowBitsLength -= length;
        return bits;
    }

    public int readRangeCoded(int length) {
        var bits = 0;
        for (int i = 0; i < length; i++) {
            bits <<= 1;
            bits |= read();
        }
        return bits;
    }
}

class BitWriteStream {
    public byte[] buffer = null;
    public int bufferIndex = 0;
    public int nowBitsIndex = 0;
    public int nowBits = 0;

    bool mIsEnd = false;

    public BitWriteStream(byte[] buffer, int bufferOffset = 0, int bitsOffset = 0) {
        this.buffer = buffer;
        bufferIndex = bufferOffset;
        nowBitsIndex = bitsOffset;
        nowBits = buffer[bufferOffset];
    }

    void write(int bit) {
        if (mIsEnd) {
            throw new Exception("Lack of data length");
        }
        bit <<= nowBitsIndex;
        nowBits += bit;
        nowBitsIndex++;
        if (nowBitsIndex >= 8) {
            buffer[bufferIndex] = (byte)nowBits;
            bufferIndex++;
            nowBits = 0;
            nowBitsIndex = 0;
            if (buffer.Length <= bufferIndex) {
                mIsEnd = true;
            }
        }
    }

    public void writeRange(int value, int length) {
        var mask = 1;
        for (int i = 0; i < length; i++) {
            var bit = 0 < (value & mask) ? 1 : 0;
            write(bit);
            mask <<= 1;
        }
    }

    public void writeRangeCoded(Code value) {
        var mask = 1 << (value.bitlen - 1);
        for (int i = 0; i < value.bitlen; i++) {
            var bit = 0 < (value.code & mask) ? 1 : 0;
            write(bit);
            mask >>= 1;
        }
    }
}

struct Simble {
    public int count;
    public List<int> simbles;
    public Simble(int c, int[] s) {
        count = c;
        simbles = new List<int>();
        simbles.AddRange(s);
    }
}

struct Code {
    public int code;
    public int bitlen;
    public Code(int c, int b) {
        code = c;
        bitlen = b;
    }
}

class LZ77 {
    const int REPEAT_LEN_MIN = 3;
    const int FAST_INDEX_CHECK_MAX = 128;
    const int FAST_INDEX_CHECK_MIN = 16;
    const int FAST_REPEAT_LENGTH = 8;

    static Dictionary<int, List<int>> generateIndexMap(byte[] input, int startIndex, int targetLength) {
        var end = startIndex + targetLength - REPEAT_LEN_MIN;
        var indexMap = new Dictionary<int, List<int>>();
        for (int i = startIndex; i <= end; i++) {
            var indexKey = input[i] << 16 | input[i + 1] << 8 | input[i + 2];
            if (indexMap.ContainsKey(indexKey)) {
                indexMap[indexKey].Add(i);
            } else {
                indexMap.Add(indexKey, new List<int>());
            }
        }
        return indexMap;
    }

    public static List<int[]> generateCodes(byte[] input, int startIndex, int targetLength) {
        var nowIndex = startIndex;
        var endIndex = startIndex + targetLength - REPEAT_LEN_MIN;
        var repeatLengthCodeValue = 0;
        var repeatDistanceCodeValue = 0;

        var indexMap = generateIndexMap(input, startIndex, targetLength);

        var startIndexMap = new Dictionary<int, int>();
        var endIndexMap = new Dictionary<int, int>();
        var codeTargetValues = new List<int[]>();
        while (nowIndex <= endIndex) {
            var indexKey = input[nowIndex] << 16 | input[nowIndex + 1] << 8 | input[nowIndex + 2];
            if (!indexMap.ContainsKey(indexKey) || indexMap[indexKey].Count <= 1) {
                codeTargetValues.Add(new int[] { input[nowIndex] });
                nowIndex++;
                continue;
            }
            var indexes = indexMap[indexKey];

            {
                var slideIndexBase = (nowIndex > 0x8000) ? nowIndex - 0x8000 : 0;
                var hasStartIndex = startIndexMap.ContainsKey(indexKey);
                var skipindexes = hasStartIndex ? startIndexMap[indexKey] : 0;
                while (indexes[skipindexes] < slideIndexBase) {
                    skipindexes = (skipindexes + 1) | 0;
                }
                if (hasStartIndex) {
                    startIndexMap[indexKey] = skipindexes;
                } else {
                    startIndexMap.Add(indexKey, skipindexes);
                }
            }
            {
                var hasEndIndex = endIndexMap.ContainsKey(indexKey);
                var skipindexes = hasEndIndex ? endIndexMap[indexKey] : 0;
                while (indexes[skipindexes] < nowIndex) {
                    skipindexes = (skipindexes + 1) | 0;
                }
                if (hasEndIndex) {
                    endIndexMap[indexKey] = skipindexes;
                } else {
                    endIndexMap.Add(indexKey, skipindexes);
                }
            }

            var repeatLengthMax = 0;
            var repeatLengthMaxIndex = 0;
            var checkCount = 0;
            var idx = endIndexMap[indexKey] - 1;
            var iMin = startIndexMap[indexKey];
        indexMapLoop:
            for (; iMin <= idx; idx--) {
                if (checkCount >= FAST_INDEX_CHECK_MAX
                    || (repeatLengthMax >= FAST_REPEAT_LENGTH && checkCount >= FAST_INDEX_CHECK_MIN)) {
                    break;
                }
                checkCount++;
                var index = indexes[idx];
                for (int j = repeatLengthMax - 1; 0 < j; j--) {
                    if (input[index + j] != input[nowIndex + j]) {
                        idx--;
                        goto indexMapLoop;
                    }
                }

                var repeatLength = 258;
                for (int j = repeatLengthMax; j <= 258; j++) {
                    if (input.Length <= (index + j) || input.Length <= (nowIndex + j) || input[index + j] != input[nowIndex + j]) {
                        repeatLength = j;
                        break;
                    }
                }
                if (repeatLengthMax < repeatLength) {
                    repeatLengthMax = repeatLength;
                    repeatLengthMaxIndex = index;
                    if (258 <= repeatLength) {
                        break;
                    }
                }
            }

            if (repeatLengthMax >= 3 && nowIndex + repeatLengthMax <= endIndex) {
                var distance = nowIndex - repeatLengthMaxIndex;
                for (int i = 0; i < Deflate.LENGTH_EXTRA_BIT_BASE.Length; i++) {
                    if (Deflate.LENGTH_EXTRA_BIT_BASE[i] > repeatLengthMax) {
                        break;
                    }
                    repeatLengthCodeValue = i;
                }
                for (int i = 0; i < Deflate.DISTANCE_EXTRA_BIT_BASE.Length; i++) {
                    if (Deflate.DISTANCE_EXTRA_BIT_BASE[i] > distance) {
                        break;
                    }
                    repeatDistanceCodeValue = i;
                }
                codeTargetValues.Add(new int[] {
                        repeatLengthCodeValue,
                        repeatDistanceCodeValue,
                        repeatLengthMax,
                        distance
                    });
                nowIndex += repeatLengthMax;
            } else {
                codeTargetValues.Add(new int[] { input[nowIndex] });
                nowIndex++;
            }
        }

        codeTargetValues.Add(new int[] { input[nowIndex] });
        codeTargetValues.Add(new int[] { input[nowIndex + 1] });
        return codeTargetValues;
    }
}

class Deflate {
    const int BLOCK_MAX_BUFFER_LEN = 131072;
    
    public static readonly int[] LENGTH_EXTRA_BIT_BASE = {
        3, 4, 5, 6, 7, 8, 9, 10, 11, 13,
        15, 17, 19, 23, 27, 31, 35, 43, 51, 59,
        67, 83, 99, 115, 131, 163, 195, 227, 258,
    };
    public static readonly int[] DISTANCE_EXTRA_BIT_BASE = {
        1, 2, 3, 4, 5, 7, 9, 13, 17, 25,
        33, 49, 65, 97, 129, 193, 257, 385, 513, 769,
        1025, 1537, 2049, 3073, 4097, 6145,
        8193, 12289, 16385, 24577,
    };
    static readonly int[] LENGTH_EXTRA_BIT_LEN = {
        0, 0, 0, 0, 0, 0, 0, 0, 1, 1,
        1, 1, 2, 2, 2, 2, 3, 3, 3, 3,
        4, 4, 4, 4, 5, 5, 5, 5, 0,
    };
    static readonly int[] DISTANCE_EXTRA_BIT_LEN = {
        0, 0, 0, 0, 1, 1, 2, 2, 3, 3,
        4, 4, 5, 5, 6, 6, 7, 7, 8, 8,
        9, 9, 10, 10, 11, 11, 12, 12, 13, 13,
    };
    static readonly int[] CODELEN_VALUES = {
        16, 17, 18, 0, 8, 7, 9, 6, 10, 5, 11, 4, 12, 3, 13, 2, 14, 1, 15,
    };

    enum BTYPE {
        UNCOMPRESSED = 0,
        FIXED = 1,
        DYNAMIC = 2
    }

    static Dictionary<int, Code> generateDeflateHuffmanTable(int[] values, int maxLength = 15) {
        var valuesCount = new Dictionary<int, int>();
        foreach (var value in values) {
            if (!valuesCount.ContainsKey(value)) {
                valuesCount.Add(value, 1);
            } else {
                valuesCount[value]++;
            }
        }

        var valuesCountKeys = valuesCount.Keys.ToArray();

        var tmpPackageIndex = 0;
        var tmpPackages = new List<Simble>();
        var packages = new List<Simble>();        
        if (valuesCountKeys.Length == 1) {
            packages.Add(new Simble(
                valuesCount[0],
                new int[] { valuesCountKeys[0] }
            ));
        } else {
            for (int i = 0; i < maxLength; i++) {
                packages = new List<Simble>();
                foreach (var value in valuesCountKeys) {
                    var pack = new Simble(
                        valuesCount[value],
                        new int[] { value }
                    );
                    packages.Add(pack);
                }

                tmpPackageIndex = 0;
                while (tmpPackageIndex + 2 <= tmpPackages.Count) {
                    var pack = new Simble();
                    pack.count = tmpPackages[tmpPackageIndex].count + tmpPackages[tmpPackageIndex + 1].count;
                    pack.simbles = new List<int>();
                    pack.simbles.AddRange(tmpPackages[tmpPackageIndex].simbles.ToArray());
                    pack.simbles.AddRange(tmpPackages[tmpPackageIndex + 1].simbles.ToArray());
                    packages.Add(pack);
                    tmpPackageIndex += 2;
                }

                packages.Sort(new Comparison<Simble>((a, b) => {
                    if (a.count < b.count) {
                        return -1;
                    }
                    if (a.count > b.count) {
                        return 1;
                    }
                    if (a.simbles.Count < b.simbles.Count) {
                        return -1;
                    }
                    if (a.simbles.Count > b.simbles.Count) {
                        return 1;
                    }
                    if (a.simbles[0] < b.simbles[0]) {
                        return -1;
                    }
                    if (a.simbles[0] > b.simbles[0]) {
                        return 1;
                    }
                    return 0;
                }));

                if (packages.Count % 2 != 0) {
                    packages.RemoveAt(packages.Count - 1);
                }
                tmpPackages = packages;
            }
        }

        var valuesCodelen = new Dictionary<int, int>();
        foreach (var pack in packages) {
            foreach (var symble in pack.simbles) {
                if (!valuesCodelen.ContainsKey(symble)) {
                    valuesCodelen.Add(symble, 1);
                } else {
                    valuesCodelen[symble]++;
                }
            };
        };

        var valuesCodelenKeys = valuesCodelen.Keys.ToArray();
        var codelenGroup = new Dictionary<int, List<int>>();
        var codelen = 3;
        var codelenValueMin = int.MaxValue;
        var codelenValueMax = 0;
        foreach (var valuesCodelenKey in valuesCodelenKeys) {
            codelen = valuesCodelen[valuesCodelenKey];
            if (!codelenGroup.ContainsKey(codelen)) {
                codelenGroup.Add(codelen, new List<int>());
                if (codelenValueMin > codelen) {
                    codelenValueMin = codelen;
                }
                if (codelenValueMax < codelen) {
                    codelenValueMax = codelen;
                }
            }
            codelenGroup[codelen].Add(valuesCodelenKey);
        };

        var table = new Dictionary<int, Code>();
        var code = 0;
        for (int i = codelenValueMin; i <= codelenValueMax; i++) {
            if (codelenGroup.ContainsKey(i)) {
                var group = codelenGroup[i];
                group.Sort(new Comparison<int>((a, b) => {
                    if (a < b) {
                        return -1;
                    }
                    if (a > b) {
                        return 1;
                    }
                    return 0;
                }));
                foreach (var value in group) {
                    table.Add(value, new Code(code, i));
                    code++;
                }
            }
            code <<= 1;
        }
        return table;
    }

    static void deflateDynamicBlock(BitWriteStream stream, byte[] input, int startIndex, int targetLength) {
        var lz77Codes = LZ77.generateCodes(input, startIndex, targetLength);
        var clCodeValues = new List<int>() { 256 }; // character or matching length
        var distanceCodeValues = new List<int>();
        var clCodeValueMax = 256;
        var distanceCodeValueMax = 0;
        for (int i = 0, iMax = lz77Codes.Count; i < iMax; i++) {
            var values = lz77Codes[i];
            var cl = values[0];
            if (2 <= values.Length) {
                cl += 257;
                var distance = values[1];
                distanceCodeValues.Add(distance);
                if (distanceCodeValueMax < distance) {
                    distanceCodeValueMax = distance;
                }
            }
            clCodeValues.Add(cl);
            if (clCodeValueMax < cl) {
                clCodeValueMax = cl;
            }
        }
        var dataHuffmanTables = generateDeflateHuffmanTable(clCodeValues.ToArray());
        var distanceHuffmanTables = generateDeflateHuffmanTable(distanceCodeValues.ToArray());

        var codelens = new List<int>();
        for (int i = 0; i <= clCodeValueMax; i++) {
            if (dataHuffmanTables.ContainsKey(i)) {
                codelens.Add(dataHuffmanTables[i].bitlen);
            } else {
                codelens.Add(0);
            }
        }

        var HLIT = codelens.Count;
        for (int i = 0; i <= distanceCodeValueMax; i++) {
            if (distanceHuffmanTables.ContainsKey(i)) {
                codelens.Add(distanceHuffmanTables[i].bitlen);
            } else {
                codelens.Add(0);
            }
        }
        var HDIST = codelens.Count - HLIT;

        var runLengthCodes = new List<int>();
        var runLengthRepeatCount = new List<int>();
        for (int i = 0; i < codelens.Count; i++) {
            var codelen = codelens[i];
            var repeatLength = 1;
            while ((i + 1) < codelens.Count && codelen == codelens[i + 1]) {
                repeatLength++;
                i++;
                if (codelen == 0) {
                    if (138 <= repeatLength) {
                        break;
                    }
                } else {
                    if (6 <= repeatLength) {
                        break;
                    }
                }
            }
            if (4 <= repeatLength) {
                if (codelen == 0) {
                    if (11 <= repeatLength) {
                        runLengthCodes.Add(18);
                    } else {
                        runLengthCodes.Add(17);
                    }
                } else {
                    runLengthCodes.Add(codelen);
                    runLengthRepeatCount.Add(1);
                    repeatLength--;
                    runLengthCodes.Add(16);
                }
                runLengthRepeatCount.Add(repeatLength);
            } else {
                for (int j = 0; j < repeatLength; j++) {
                    runLengthCodes.Add(codelen);
                    runLengthRepeatCount.Add(1);
                }
            }
        }

        var codelenHuffmanTable = generateDeflateHuffmanTable(runLengthCodes.ToArray(), 7);
        var HCLEN = 0;
        for (int i = 0; i < CODELEN_VALUES.Length; i++) {
            if (codelenHuffmanTable.ContainsKey(CODELEN_VALUES[i])) {
                HCLEN = i + 1;
            }
        }

        // HLIT
        stream.writeRange(HLIT - 257, 5);
        // HDIST
        stream.writeRange(HDIST - 1, 5);
        // HCLEN
        stream.writeRange(HCLEN - 4, 4);

        // codelenHuffmanTable
        for (int i = 0; i < HCLEN; i++) {
            if (codelenHuffmanTable.ContainsKey(CODELEN_VALUES[i])) {
                var codelenTableObj = codelenHuffmanTable[CODELEN_VALUES[i]];
                stream.writeRange(codelenTableObj.bitlen, 3);
            } else {
                stream.writeRange(0, 3);
            }
        }

        for (int i = 0; i < runLengthCodes.Count; i++) {
            var value = runLengthCodes[i];
            if (codelenHuffmanTable.ContainsKey(value)) {
                var codelenTableObj = codelenHuffmanTable[value];
                stream.writeRangeCoded(codelenTableObj);
            } else {
                throw new Exception("Data is corrupted");
            }
            if (value == 18) {
                stream.writeRange(runLengthRepeatCount[i] - 11, 7);
            } else if (value == 17) {
                stream.writeRange(runLengthRepeatCount[i] - 3, 3);
            } else if (value == 16) {
                stream.writeRange(runLengthRepeatCount[i] - 3, 2);
            }
        }

        for (int i = 0, iMax = lz77Codes.Count; i < iMax; i++) {
            var values = lz77Codes[i];
            var clCodeValue = values[0];
            if (2 <= values.Length) {
                var distanceCodeValue = values[1];
                if (!dataHuffmanTables.ContainsKey(clCodeValue + 257)) {
                    throw new Exception("Data is corrupted");
                }
                var codelenTableObj = dataHuffmanTables[clCodeValue + 257];
                stream.writeRangeCoded(codelenTableObj);
                if (0 < LENGTH_EXTRA_BIT_LEN[clCodeValue]) {
                    var repeatLength = values[2];
                    stream.writeRange(
                        repeatLength - LENGTH_EXTRA_BIT_BASE[clCodeValue],
                        LENGTH_EXTRA_BIT_LEN[clCodeValue]
                    );
                }
                if (!distanceHuffmanTables.ContainsKey(distanceCodeValue)) {
                    throw new Exception("Data is corrupted");
                }
                var distanceTableObj = distanceHuffmanTables[distanceCodeValue];
                stream.writeRangeCoded(distanceTableObj);
                if (0 < DISTANCE_EXTRA_BIT_LEN[distanceCodeValue]) {
                    var distance = values[3];
                    stream.writeRange(
                        distance - DISTANCE_EXTRA_BIT_BASE[distanceCodeValue],
                        DISTANCE_EXTRA_BIT_LEN[distanceCodeValue]
                    );
                }
            } else {
                if (!dataHuffmanTables.ContainsKey(clCodeValue)) {
                    throw new Exception("Data is corrupted");
                }
                var codelenTableObj = dataHuffmanTables[clCodeValue];
                stream.writeRangeCoded(codelenTableObj);
            }
        }
        if (!dataHuffmanTables.ContainsKey(256)) {
            throw new Exception("Data is corrupted");
        }
        var codelenTable256 = dataHuffmanTables[256];
        stream.writeRangeCoded(codelenTable256);
    }

    static Dictionary<int, Dictionary<int, int>> FIXED_HUFFMAN_TABLE
        = generateHuffmanTable(makeFixedHuffmanCodelenValues());

    static Dictionary<int, Dictionary<int, int>> generateHuffmanTable(Dictionary<int, List<int>> codelenValues) {
        var codelens = codelenValues.Keys;
        var codelen = 0;
        var codelenMax = 0;
        var codelenMin = int.MaxValue;
        foreach(var key in codelens) {
            codelen = key;
            if (codelenMax < codelen) {
                codelenMax = codelen;
            }
            if (codelenMin > codelen) {
                codelenMin = codelen;
            }
        }

        var code = 0;
        var bitlenTables = new Dictionary<int, Dictionary<int, int>>();
        for (int bitlen = codelenMin; bitlen <= codelenMax; bitlen++) {
            List<int> values;
            if (codelenValues.ContainsKey(bitlen)) {
                values = codelenValues[bitlen];
            } else {
                values = new List<int>();
            }
            
            values.Sort(new Comparison<int>((a, b) => {
                if (a < b) {
                    return -1;
                }
                if (a > b) {
                    return 1;
                }
                return 0;
            }));

            var table = new Dictionary<int, int>();
            foreach(var value in values) {
                table.Add(code, value);
                code++;
            }
            bitlenTables.Add(bitlen, table);
            code <<= 1;
        }
        return bitlenTables;
    }

    static Dictionary<int, List<int>> makeFixedHuffmanCodelenValues() {
        var codelenValues = new Dictionary<int, List<int>>();
        codelenValues.Add(7, new List<int>());
        codelenValues.Add(8, new List<int>());
        codelenValues.Add(9, new List<int>());
        for (int i = 0; i <= 287; i++) {
            if (i <= 143) {
                codelenValues[8].Add(i);
            } else if (i <= 255) {
                codelenValues[9].Add(i);
            } else if (i <= 279) {
                codelenValues[7].Add(i);
            } else {
                codelenValues[8].Add(i);
            }
        }
        return codelenValues;
    }

    static void inflateUncompressedBlock(BitReadStream stream, Uint8WriteStream buffer) {
        // Skip to byte boundary
        if (stream.nowBitsLength < 8) {
            stream.readRange(stream.nowBitsLength);
        }
        var LEN = stream.readRange(8) | stream.readRange(8) << 8;
        var NLEN = stream.readRange(8) | stream.readRange(8) << 8;
        if ((LEN + NLEN) != 65535) {
            throw new Exception("Data is corrupted");
        }
        for (int i = 0; i < LEN; i++) {
            buffer.write((byte)stream.readRange(8));
        }
    }

    static void inflateFixedBlock(BitReadStream stream, Uint8WriteStream buffer) {
        var tables = FIXED_HUFFMAN_TABLE;
        var codelens = tables.Keys.ToArray();
        var codelen = 0;
        var codelenMax = 0;
        var codelenMin = int.MaxValue;
        foreach (var key in codelens) {
            codelen = key;
            if (codelenMax < codelen) {
                codelenMax = codelen;
            }
            if (codelenMin > codelen) {
                codelenMin = codelen;
            }
        }
        var code = 0;
        int value;
        int repeatLengthCode;
        int repeatLengthValue;
        int repeatLengthExt;
        int repeatDistanceCode;
        int repeatDistanceValue;
        int repeatDistanceExt;
        int repeatStartIndex;
        while (!stream.isEnd) {
            codelen = codelenMin;
            code = stream.readRangeCoded(codelenMin);
            while (true) {
                if (tables.ContainsKey(codelen) && tables[codelen].ContainsKey(code)) {
                    value = tables[codelen][code];
                    break;
                }
                if (codelenMax <= codelen) {
                    throw new Exception("Data is corrupted");
                }
                codelen++;
                code <<= 1;
                code |= stream.read();
            }
            if (value < 256) {
                buffer.write((byte)value);
                continue;
            }
            if (value == 256) {
                break;
            }
            repeatLengthCode = value - 257;
            repeatLengthValue = LENGTH_EXTRA_BIT_BASE[repeatLengthCode];
            repeatLengthExt = LENGTH_EXTRA_BIT_LEN[repeatLengthCode];
            if (0 < repeatLengthExt) {
                repeatLengthValue += stream.readRange(repeatLengthExt);
            }
            repeatDistanceCode = stream.readRangeCoded(5);
            repeatDistanceValue = DISTANCE_EXTRA_BIT_BASE[repeatDistanceCode];
            repeatDistanceExt = DISTANCE_EXTRA_BIT_LEN[repeatDistanceCode];
            if (0 < repeatDistanceExt) {
                repeatDistanceValue += stream.readRange(repeatDistanceExt);
            }
            repeatStartIndex = buffer.index - repeatDistanceValue;
            for (int i = 0; i < repeatLengthValue; i++) {
                buffer.write(buffer.buffer[repeatStartIndex + i]);
            }
        }
    }

    static void inflateDynamicBlock(BitReadStream stream, Uint8WriteStream buffer) {
        var HLIT = stream.readRange(5) + 257;
        var HDIST = stream.readRange(5) + 1;
        var HCLEN = stream.readRange(4) + 4;
        var codelenCodelen = 0;
        var codelenCodelenValues = new Dictionary<int, List<int>>();
        for (int i = 0; i < HCLEN; i++) {
            codelenCodelen = stream.readRange(3);
            if (codelenCodelen == 0) {
                continue;
            }
            if (!codelenCodelenValues.ContainsKey(codelenCodelen)) {
                codelenCodelenValues.Add(codelenCodelen, new List<int>());
            }
            codelenCodelenValues[codelenCodelen].Add(CODELEN_VALUES[i]);
        }
        var codelenHuffmanTables = generateHuffmanTable(codelenCodelenValues);
        var codelenCodelens = codelenHuffmanTables.Keys.ToArray();
        var codelenCodelenMax = 0;
        var codelenCodelenMin = int.MaxValue;
        foreach (var key in codelenCodelens) {
            codelenCodelen = key;
            if (codelenCodelenMax < codelenCodelen) {
                codelenCodelenMax = codelenCodelen;
            }
            if (codelenCodelenMin > codelenCodelen) {
                codelenCodelenMin = codelenCodelen;
            }
        }
        var dataCodelenValues = new Dictionary<int, List<int>>();
        var distanceCodelenValues = new Dictionary<int, List<int>>();
        var codelenCode = 0;
        var runlengthCode = 0;
        var repeat = 0;
        var codelen = 0;
        var codesNumber = HLIT + HDIST;
        for (int i = 0; i < codesNumber;) {
            codelenCodelen = codelenCodelenMin;
            codelenCode = stream.readRangeCoded(codelenCodelenMin);
            while (true) {
                if (codelenHuffmanTables.ContainsKey(codelenCodelen) &&
                    codelenHuffmanTables[codelenCodelen].ContainsKey(codelenCode)) {
                    runlengthCode = codelenHuffmanTables[codelenCodelen][codelenCode];
                    break;
                }
                if (codelenCodelenMax <= codelenCodelen) {
                    throw new Exception("Data is corrupted");
                }
                codelenCodelen++;
                codelenCode <<= 1;
                codelenCode |= stream.read();
            }
            if (runlengthCode == 16) {
                repeat = 3 + stream.readRange(2);
            } else if (runlengthCode == 17) {
                repeat = 3 + stream.readRange(3);
                codelen = 0;
            } else if (runlengthCode == 18) {
                repeat = 11 + stream.readRange(7);
                codelen = 0;
            } else {
                repeat = 1;
                codelen = runlengthCode;
            }
            if (codelen <= 0) {
                i += repeat;
            } else {
                while (0 < repeat) {
                    if (i < HLIT) {
                        if (!dataCodelenValues.ContainsKey(codelen)) {
                            dataCodelenValues.Add(codelen, new List<int>());
                        }
                        dataCodelenValues[codelen].Add(i++);
                    } else {
                        if (!distanceCodelenValues.ContainsKey(codelen)) {
                            distanceCodelenValues.Add(codelen, new List<int>());
                        }
                        distanceCodelenValues[codelen].Add(i++ - HLIT);
                    }
                    repeat--;
                }
            }
        }

        var dataHuffmanTables = generateHuffmanTable(dataCodelenValues);
        var distanceHuffmanTables = generateHuffmanTable(distanceCodelenValues);
        var dataCodelens = dataHuffmanTables.Keys.ToArray();
        var dataCodelen = 0;
        var dataCodelenMax = 0;
        var dataCodelenMin = int.MaxValue;
        foreach (var key in dataCodelens) {
            dataCodelen = key;
            if (dataCodelenMax < dataCodelen) {
                dataCodelenMax = dataCodelen;
            }
            if (dataCodelenMin > dataCodelen) {
                dataCodelenMin = dataCodelen;
            }
        }

        var distanceCodelens = distanceHuffmanTables.Keys.ToArray();
        var distanceCodelen = 0;
        var distanceCodelenMax = 0;
        var distanceCodelenMin = int.MaxValue;
        foreach (var key in distanceCodelens) {
            distanceCodelen = key;
            if (distanceCodelenMax < distanceCodelen) {
                distanceCodelenMax = distanceCodelen;
            }
            if (distanceCodelenMin > distanceCodelen) {
                distanceCodelenMin = distanceCodelen;
            }
        }

        var dataCode = 0;
        int data;
        int repeatLengthCode;
        int repeatLengthValue;
        int repeatLengthExt;
        int repeatDistanceCode;
        int repeatDistanceValue;
        int repeatDistanceExt;
        int repeatDistanceCodeCodelen;
        int repeatDistanceCodeCode;
        int repeatStartIndex;
        while (!stream.isEnd) {
            dataCodelen = dataCodelenMin;
            dataCode = stream.readRangeCoded(dataCodelenMin);
            while (true) {
                if (dataHuffmanTables.ContainsKey(dataCodelen) &&
                    dataHuffmanTables[dataCodelen].ContainsKey(dataCode)) {
                    data = dataHuffmanTables[dataCodelen][dataCode];
                    break;
                }
                if (dataCodelenMax <= dataCodelen) {
                    throw new Exception("Data is corrupted");
                }
                dataCodelen++;
                dataCode <<= 1;
                dataCode |= stream.read();
            }
            if (data < 256) {
                buffer.write(data);
                continue;
            }
            if (data == 256) {
                break;
            }
            repeatLengthCode = data - 257;
            repeatLengthValue = LENGTH_EXTRA_BIT_BASE[repeatLengthCode];
            repeatLengthExt = LENGTH_EXTRA_BIT_LEN[repeatLengthCode];
            if (0 < repeatLengthExt) {
                repeatLengthValue += stream.readRange(repeatLengthExt);
            }
            repeatDistanceCodeCodelen = distanceCodelenMin;
            repeatDistanceCodeCode = stream.readRangeCoded(distanceCodelenMin);
            while (true) {
                if (distanceHuffmanTables.ContainsKey(repeatDistanceCodeCodelen) &&
                    distanceHuffmanTables[repeatDistanceCodeCodelen].ContainsKey(repeatDistanceCodeCode)) {
                    repeatDistanceCode = distanceHuffmanTables[repeatDistanceCodeCodelen][repeatDistanceCodeCode];
                    break;
                }
                if (distanceCodelenMax <= repeatDistanceCodeCodelen) {
                    throw new Exception("Data is corrupted");
                }
                repeatDistanceCodeCodelen++;
                repeatDistanceCodeCode <<= 1;
                repeatDistanceCodeCode |= stream.read();
            }
            repeatDistanceValue = DISTANCE_EXTRA_BIT_BASE[repeatDistanceCode];
            repeatDistanceExt = DISTANCE_EXTRA_BIT_LEN[repeatDistanceCode];
            if (0 < repeatDistanceExt) {
                repeatDistanceValue += stream.readRange(repeatDistanceExt);
            }
            repeatStartIndex = buffer.index - repeatDistanceValue;
            for (int i = 0; i < repeatLengthValue; i++) {
                buffer.write(buffer.buffer[repeatStartIndex + i]);
            }
        }
    }

    public static byte[] Compress(byte[] input) {
        var inputLength = input.Length;
        var streamHeap = (inputLength < BLOCK_MAX_BUFFER_LEN / 2) ? BLOCK_MAX_BUFFER_LEN : inputLength * 2;
        var stream = new BitWriteStream(new byte[streamHeap]);
        var processedLength = 0;
        var targetLength = 0;
        while (true) {
            if (processedLength + BLOCK_MAX_BUFFER_LEN >= inputLength) {
                targetLength = inputLength - processedLength;
                stream.writeRange(1, 1);
            } else {
                targetLength = BLOCK_MAX_BUFFER_LEN;
                stream.writeRange(0, 1);
            }
            stream.writeRange((int)BTYPE.DYNAMIC, 2);
            deflateDynamicBlock(stream, input, processedLength, targetLength);
            processedLength += BLOCK_MAX_BUFFER_LEN;
            if (processedLength >= inputLength) {
                break;
            }
        }
        if (stream.nowBitsIndex != 0) {
            stream.writeRange(0, 8 - stream.nowBitsIndex);
        }
        var ret = new byte[stream.bufferIndex];
        Array.Copy(stream.buffer, 0, ret, 0, ret.Length);
        return ret;
    }

    public static byte[] UnCompress(byte[] input, int offset = 0) {
        var buffer = new Uint8WriteStream(input.Length * 10);
        var stream = new BitReadStream(input, offset);
        var bFinal = 0;
        while (bFinal != 1) {
            bFinal = stream.readRange(1);
            var bType = (BTYPE)stream.readRange(2);
            if (bType == BTYPE.UNCOMPRESSED) {
                inflateUncompressedBlock(stream, buffer);
            } else if (bType == BTYPE.FIXED) {
                inflateFixedBlock(stream, buffer);
            } else if (bType == BTYPE.DYNAMIC) {
                inflateDynamicBlock(stream, buffer);
            } else {
                throw new Exception("Not supported BTYPE : " + bType);
            }
            if (bFinal == 0 && stream.isEnd) {
                throw new Exception("Data length is insufficient");
            }
        }

        var ret = new byte[buffer.index];
        Array.Copy(buffer.buffer, 0, ret, 0, ret.Length);
        return ret;
    }
}
