using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deflate {
    class Deflate {
        const bool DEBUG = false;
        const int BLOCK_MAX_BUFFER_LEN = 131072;
        const int REPEAT_LEN_MIN = 3;
        const int FAST_INDEX_CHECK_MAX = 128;
        const int FAST_INDEX_CHECK_MIN = 16;
        const int FAST_REPEAT_LENGTH = 8;
        static readonly int[] CODELEN_VALUES = {
            16, 17, 18, 0, 8, 7, 9, 6, 10, 5, 11, 4, 12, 3, 13, 2, 14, 1, 15,
        };
        static readonly int[] LENGTH_EXTRA_BIT_BASE = {
            3, 4, 5, 6, 7, 8, 9, 10, 11, 13,
            15, 17, 19, 23, 27, 31, 35, 43, 51, 59,
            67, 83, 99, 115, 131, 163, 195, 227, 258,
        };
        static readonly int[] LENGTH_EXTRA_BIT_LEN = {
            0, 0, 0, 0, 0, 0, 0, 0, 1, 1,
            1, 1, 2, 2, 2, 2, 3, 3, 3, 3,
            4, 4, 4, 4, 5, 5, 5, 5, 0,
        };
        static readonly int[] DISTANCE_EXTRA_BIT_BASE = {
            1, 2, 3, 4, 5, 7, 9, 13, 17, 25,
            33, 49, 65, 97, 129, 193, 257, 385, 513, 769,
            1025, 1537, 2049, 3073, 4097, 6145,
            8193, 12289, 16385, 24577,
        };
        static readonly int[] DISTANCE_EXTRA_BIT_LEN = {
            0, 0, 0, 0, 1, 1, 2, 2, 3, 3,
            4, 4, 5, 5, 6, 6, 7, 7, 8, 8,
            9, 9, 10, 10, 11, 11, 12, 12, 13, 13,
        };
        enum BTYPE {
            UNCOMPRESSED = 0,
            FIXED = 1,
            DYNAMIC = 2
        }

        class BitWriteStream {
            public byte[] buffer;
            public int bufferIndex;
            public int nowBitsIndex;

            bool isEnd = false;
            int nowBits = 0;

            public BitWriteStream(byte[] buffer, int bufferOffset = 0, int bitsOffset = 0) {
                this.buffer = buffer;
                bufferIndex = bufferOffset;
                nowBitsIndex = bitsOffset;
                nowBits = buffer[bufferOffset];
            }

            public void writeRange(int value, int length) {
                var mask = 1;
                for (int i = 0; i < length; i++) {
                    var bit = (byte)(0 < (value & mask) ? 1 : 0);
                    write(bit);
                    mask <<= 1;
                }
            }

            public void writeRangeCoded(Code code) {
                var mask = 1 << (code.bitlen - 1);
                for (int i = 0; i < code.bitlen; i++) {
                    var bit = (byte)(0 < (code.code & mask) ? 1 : 0);
                    write(bit);
                    mask >>= 1;
                }
            }

            void write(byte bit) {
                if (isEnd) {
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
                        isEnd = true;
                    }
                }
            }
        }

        struct Simble {
            public int count;
            public List<int> simbles;
            public Simble(int count, int[] simbles) {
                this.count = count;
                this.simbles = new List<int>();
                this.simbles.AddRange(simbles);
            }
        }

        struct Code {
            public int code;
            public int bitlen;
            public Code(int c, int l) {
                code = c;
                bitlen = l;
            }
        }

        static string dump(int[] arr, bool flat = false) {
            if (flat) {
                var str = "";
                for (int i = 0; i < arr.Length; i++) {
                    var v = arr[i];
                    var val = ("" + v).PadLeft(3, '0');
                    str += val + "\r\n";
                }
                return str;
            }
            var val16 = "";
            var chr16 = "";
            for (int i = 0; i < arr.Length; i++) {
                var v = arr[i];
                var val = ("" + v).PadLeft(3, '0');
                var addr = i.ToString("X").PadLeft(4, '0');
                var chr = (0x20 <= v && v <= 0x7F) ? Encoding.ASCII.GetString(new byte[] { (byte)v }) : "ä";
                chr16 += chr;
                switch (i % 16) {
                case 0:
                    val16 += addr + "　" + val + " ";
                    break;
                case 3:
                case 7:
                case 11:
                    val16 += val + "　";
                    break;
                case 15:
                    val16 += val + "　" + chr16 + "\r\n";
                    chr16 = "";
                    break;
                default:
                    val16 += val + " ";
                    break;
                }
            }
            return val16;
        }

        static string dump(byte[] arr) {
            var val16 = "";
            var chr16 = "";
            for (int i = 0; i < arr.Length; i++) {
                var v = arr[i];
                var val = v.ToString("X").PadLeft(2, '0');
                var addr = i.ToString("X").PadLeft(4, '0');
                var chr = (0x20 <= v && v <= 0x7F) ? Encoding.ASCII.GetString(new byte[] { v }) : "ä";
                chr16 += chr;
                switch (i % 16) {
                case 0:
                    val16 += addr + "　" + val + " ";
                    break;
                case 3:
                case 7:
                case 11:
                    val16 += val + "　";
                    break;
                case 15:
                    val16 += val + "　" + chr16 + "\r\n";
                    chr16 = "";
                    break;
                default:
                    val16 += val + " ";
                    break;
                }
            }
            return val16;
        }

        static Dictionary<int, List<int>> generateLZ77IndexMap(byte[] input, int startIndex, int targetLength) {
            var end = startIndex + targetLength - REPEAT_LEN_MIN;
            var indexMap = new Dictionary<int, List<int>>();
            for (int i = startIndex; i <= end; i++) {
                var indexKey = input[i] << 16 | input[i + 1] << 8 | input[i + 2];
                if (!indexMap.ContainsKey(indexKey)) {
                    indexMap[indexKey] = new List<int>();
                }
                indexMap[indexKey].Add(i);
            }
            
            return indexMap;
        }

        static List<int[]> generateLZ77Codes(byte[] input, int startIndex, int targetLength) {
            var nowIndex = startIndex;
            var endIndex = startIndex + targetLength - REPEAT_LEN_MIN;
            var slideIndexBase = 0;
            var repeatLength = 0;
            var repeatLengthMax = 0;
            var repeatLengthMaxIndex = 0;
            var distance = 0;
            var repeatLengthCodeValue = 0;
            var repeatDistanceCodeValue = 0;

            var codeTargetValues = new List<int[]>();
            var startIndexMap = new Dictionary<int, int>();
            var endIndexMap = new Dictionary<int, int>();

            var indexMap = generateLZ77IndexMap(input, startIndex, targetLength);
            while (nowIndex <= endIndex) {
                var indexKey = input[nowIndex] << 16 | input[nowIndex + 1] << 8 | input[nowIndex + 2];
                if (!indexMap.ContainsKey(indexKey) || indexMap[indexKey].Count <= 1) {
                    codeTargetValues.Add(new int[] { input[nowIndex] });
                    nowIndex++;
                    continue;
                }
                var indexes = indexMap[indexKey];

                slideIndexBase = (nowIndex > 0x8000) ? nowIndex - 0x8000 : 0;
                repeatLengthMax = 0;
                repeatLengthMaxIndex = 0;
                //???var skipindexes = startIndexMap[indexKey] || 0;
                var skipindexes = startIndexMap.ContainsKey(indexKey) ? startIndexMap[indexKey] : 0;
                while (indexes[skipindexes] < slideIndexBase) {
                    skipindexes = (skipindexes + 1) | 0;
                }
                startIndexMap[indexKey] = skipindexes;
                //???skipindexes = endIndexMap[indexKey] || 0;
                skipindexes = endIndexMap.ContainsKey(indexKey) ? endIndexMap[indexKey] : 0;
                while (indexes[skipindexes] < nowIndex) {
                    skipindexes = (skipindexes + 1) | 0;
                }
                endIndexMap[indexKey] = skipindexes;

                var checkCount = 0;
                int idx = endIndexMap[indexKey] - 1;
                int iMin = startIndexMap[indexKey];
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
                            //??? continue indexMapLoop;
                            idx--;
                            goto indexMapLoop;
                        }
                    }
                    repeatLength = 258;
                    for (int j = repeatLengthMax; j <= 258; j++) {
                        if (input[index + j] != input[nowIndex + j]) {
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
                    distance = nowIndex - repeatLengthMaxIndex;
                    for (int i = 0; i < LENGTH_EXTRA_BIT_BASE.Length; i++) {
                        if (LENGTH_EXTRA_BIT_BASE[i] > repeatLengthMax) {
                            break;
                        }
                        repeatLengthCodeValue = i;
                    }
                    for (int i = 0; i < DISTANCE_EXTRA_BIT_BASE.Length; i++) {
                        if (DISTANCE_EXTRA_BIT_BASE[i] > distance) {
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

        static Dictionary<int, Code> generateDeflateHuffmanTable(int[] values, int maxLength = 15) {
            var valuesCount = new Dictionary<int, int>();
            foreach (var value in values) {
                if (!valuesCount.ContainsKey(value)) {
                    valuesCount.Add(value, 1);
                } else {
                    valuesCount[value]++;
                }
            }
            if (DEBUG) {
                Console.WriteLine("valuesCount");
                for (int m = 0; m < valuesCount.Count; m++) {
                    var o = valuesCount.ElementAt(m);
                    Console.Write("{0}:{1} ",
                        o.Key.ToString().PadLeft(3, '0'),
                        o.Value.ToString().PadLeft(4, '0')
                    );
                    if (m % 16 == 15) {
                        Console.WriteLine();
                    }
                }
                Console.WriteLine();
            }

            var tmpPackages = new List<Simble>();
            var tmpPackageIndex = 0;
            var packages = new List<Simble>();
            var valuesCountKeys = valuesCount.Keys.ToArray();
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
                    tmpPackages.Clear();
                    tmpPackages.AddRange(packages.ToArray());
                }
            }
            if (DEBUG) {
                Console.WriteLine("packages");
                foreach (var pack in packages) {
                    var debug = (pack.count + "").PadLeft(4, '0');
                    debug += ":[";
                    foreach (var symble in pack.simbles) {
                        debug += (symble + "").PadLeft(3, '0') + " ";
                    }
                    debug = debug.Substring(0, debug.Length - 1);
                    debug += "]";
                    Console.WriteLine(debug);
                };
                Console.WriteLine();
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
                    codelenGroup[codelen] = new List<int>();
                    if (codelenValueMin > codelen) {
                        codelenValueMin = codelen;
                    }
                    if (codelenValueMax < codelen) {
                        codelenValueMax = codelen;
                    }
                }
                codelenGroup[codelen].Add(valuesCodelenKey);
            };

            var code = 0;
            var table = new Dictionary<int, Code>();
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
            var lz77Codes = generateLZ77Codes(input, startIndex, targetLength);
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

            if (DEBUG) {
                var flat = false;
                Console.WriteLine("clCodeValueMax:" + clCodeValueMax);
                Console.WriteLine(dump(clCodeValues.ToArray(), flat));
                Console.WriteLine();
                Console.WriteLine("distanceCodeValueMax:" + distanceCodeValueMax);
                Console.WriteLine(dump(distanceCodeValues.ToArray(), flat));
                Console.WriteLine();
                Console.WriteLine("dataHuffmanTables");
                var debug = "";
                for (int m = 0; m < dataHuffmanTables.Count; m++) {
                    if (dataHuffmanTables.ContainsKey(m)) {
                        var o = dataHuffmanTables[m];
                        debug += o.code + ":" + o.bitlen + " ";
                    }
                }
                Console.WriteLine(debug);
                Console.WriteLine("distanceHuffmanTables");
                debug = "";
                for (int m = 0; m < distanceHuffmanTables.Count; m++) {
                    if (distanceHuffmanTables.ContainsKey(m)) {
                        var o = distanceHuffmanTables[m];
                        debug += o.code + ":" + o.bitlen + " ";
                    }
                }
                Console.WriteLine(debug);
            }

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
                    stream.writeRange(codelenHuffmanTable[CODELEN_VALUES[i]].bitlen, 3);
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
            Console.WriteLine(dump(ret));
            return ret;
        }
    }
}
