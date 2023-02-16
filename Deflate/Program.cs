﻿using System;
using System.Text;
using System.IO;

namespace Main {
    internal class Program {
        static byte[] encData = new byte[] {
                    42,32,32,32,
    32,32,32,84,
    10,10,32,109,
    111,115,116,32,

    115,116,114,97,
    105,103,104,116,
    102,111,114,119,
    97,114,100,32,

    116,101,99,104,
    110,105,113,117,
    101,32,116,117,
    114,110,115,32,

    111,117,116,32,
    116,111,32,98,
    101,32,116,104,
    101,32,102,97,

    115,116,101,115,
    116,32,102,111,
    114,13,10,32,
    42,32,32,32,

    32,32,32,109,
    111,115,116,32,
    105,110,112,117,
    116,32,102,105,

    108,101,115,58,
    32,116,114,121,
    32,97,108,108,
    32,112,111,115,

    115,105,98,108,
    101,32,109,97,
    116,99,104,101,
    115,32,97,110,

    100,32,115,101,
    108,101,99,116,
    32,116,104,101,
    32,108,111,110,

    103,101,115,116,
    46,13,10,32,
    42,32,32,32,
    32,32,32,84,

    104,101,32,107,
    101,121,32,102,
    101,97,116,117,
    114,101,32,111,

    102,32,116,104,
    105,115,32,97,
    108,103,111,114,
    105,116,104,109,

    32,105,115,32,
    116,104,97,116,
    32,105,110,115,
    101,114,116,105,

    111,110,115,32,
    105,110,116,111,
    32,116,104,101,
    32,115,116,114,

    105,110,103,13,
    10,32,42,32,
    32,32,32,32,
    32,100,105,99,

    116,105,111,110,
    97,114,121,32,
    97,114,101,32,
    118,101,114,121,

    32,115,105,109,
    112,108,101,32,
    97,110,100,32,
    116,104,117,115,

    32,102,97,115,
    116,44,32,97,
    110,100,32,100,
    101,108,101,116,

    105,111,110,115,
    32,97,114,101,
    32,97,118,111,
    105,100,101,100,

    13,10,32,42,
    32,32,32,32,
    32,32,99,111,
    109,112,108,101,

    116,101,108,121,
    46,32,73,110,
    115,101,114,116,
    105,111,110,115,

    32,97,114,101,
    32,112,101,114,
    102,111,114,109,
    101,100,32,97,

    116,32,101,97,
    99,104,32,105,
    110,112,117,116,
    32,99,104,97,

    114,97,99,116,
    101,114,44,32,
    119,104,101,114,
    101,97,115,13,

    10,32,42,32,
    32,32,32,32,
    32,115,116,114,
    105,110,103,32,

    109,97,116,99,
    104,101,115,32,
    97,114,101,32,
    112,101,114,102,

    111,114,109,101,
    100,32,111,110,
    108,121,32,119,
    104,101,110,32,

    116,104,101,32,
    112,114,101,118,
    105,111,117,115,
    32,109,97,116,

    99,104,32,101,
    110,100,115,46,
    32,83,111,32,
    105,116,13,10,

    32,42,32,32,
    32,32,32,32,
    105,115,32,112,
    114,101,102,101,

    114,97,98,108,
    101,32,116,111,
    32,115,112,101,
    110,100,32,109,

    111,114,101,32,
    116,105,109,101,
    32,105,110,32,
    109,97,116,99,

    104,101,115,32,
    116,111,32,97,
    108,108,111,119,
    32,118,101,114,

    121,32,102,97,
    115,116,32,115,
    116,114,105,110,
    103,13,10,32,

    42,32,32,32,32,32,32,105,110,115,101,114,116,105,111,110,115,32,97,110,100,32,97,118,111,105,100,32,100,101,108,101,116,105,111,110,115,46,32,84,104,101,32,109,97,116,99,104,105,110,103,32,97,108,103,111,114,105,116,104,109,32,102,111,114,32,115,109,97,108,108,13,10,32,42,32,32,32,32,32,32,115,116,114,105,110,103,115,32,105,115,32,105,110,115,112,105,114,101,100,32,102,114,111,109,32,116,104,97,116,32,111,102,32,82,97,98,105,110,32,38,32,75,97,114,112,46,32,65,32,98,114,117,116,101,32,102,111,114,99,101,32,97,112,112,114,111,97,99,104,13,10,32,42,32,32,32,32,32,32,105,115,32,117,115,101,100,32,116,111,32,102,105,110,100,32,108,111,110,103,101,114,32,115,116,114,105,110,103,115,32,119,104,101,110,32,97,32,115,109,97,108,108,32,109,97,116,99,104,32,104,97,115,32,98,101,101,110,32,102,111,117,110,100,46,13,10,32,42,32,32,32,32,32,32,65,32,115,105,109,105,108,97,114,32,97,108,103,111,114,105,116,104,109,32,105,115,32,117,115,101,100,32,105,110,32,99,111,109,105,99,32,40,98,121,32,74,97,110,45,77,97,114,107,32,87,97,109,115,41,32,97,110,100,32,102,114,101,101,122,101,13,10,32,42,32,32,32,32,32,32,40,98,121,32,76,101,111,110,105,100,32,66,114,111,117,107,104,105,115,41,46,13,10,32,42,32,32,32,32,32,32,32,32,32,65,32,112,114,101,118,105,111,117,115,32,118,101,114,115,105,111,110,32,111,102,32,116,104,105,115,32,102,105,108,101,32,117,115,101,100,32,97,32,109,111,114,101,32,115,111,112,104,105,115,116,105,99,97,116,101,100,32,97,108,103,111,114,105,116,104,109,13,10,32,42,32,32,32,32,32,32,40,98,121,32,70,105,97,108,97,32,97,110,100,32,71,114,101,101,110,101,41,32,119,104,105,99,104,32,105,115,32,103,117,97,114,97,110,116,101,101,100,32,116,111,32,114,117,110,32,105,110,32,108,105,110,101,97,114,32,97,109,111,114,116,105,122,101,100,13,10,32,42,32,32,32,32,32,32,116,105,109,101,44,32,98,117,116,32,104,97,115,32,97,32,108,97,114,103,101,114,32,97,118,101,114,97,103,101,32,99,111,115,116,44,32,117,115,101,115,32,109,111,114,101,32,109,101,109,111,114,121,32,97,110,100,32,105,115,32,112,97,116,101,110,116,101,100,46,13,10,32,42,32,32,32,32,32,32,72,111,119,101,118,101,114,32,116,104,101,32,70,38,71,32,97,108,103,111,114,105,116,104,109,32,109,97,121,32,98,101,32,102,97,115,116,101,114,32,102,111,114,32,115,111,109,101,32,104,105,103,104,108,121,32,114,101,100,117,110,100,97,110,116,13,10,32,42,32,32,32,32,32,32,102,105,108,101,115,32,105,102,32,116,104,101,32,112,97,114,97,109,101,116,101,114,32,109,97,120,95,99,104,97,105,110,95,108,101,110,103,116,104,32,40,100,101,115,99,114,105,98,101,100,32,98,101,108,111,119,41,32,105,115,32,116,111,111,32,108,97,114,103,101,46
            };
        static byte[] decData = new byte[] {
0xCD, 0x5A, 0x5B, 0x6F, 0x1D, 0xB7, 0x11, 0x7E, 0x3F, 0xBF, 0x62, 0x9F, 0x0B, 0x94,
0xE6, 0x0C, 0xEF, 0x80, 0x61, 0x40, 0x96, 0xAC, 0xA0, 0x0F, 0x01, 0xDA, 0x46, 0x40, 0xFA, 0x1A,
0xA8, 0x49, 0x8A, 0xC2, 0x49, 0x9B, 0x34, 0x05, 0xFA, 0xF3, 0xFB, 0xCD, 0xF0, 0xB2, 0xA4, 0xA4,
0x13, 0xED, 0x51, 0x2C, 0xC4, 0x36, 0x8E, 0xBD, 0x3B, 0xCB, 0x21, 0xE7, 0xF2, 0xCD, 0x85, 0xDC,
0x35, 0xEC, 0x8A, 0xFE, 0xD9, 0x2C, 0xFE, 0xFE, 0xD1, 0x4C, 0xB7, 0xD9, 0x93, 0x29, 0x54, 0x4A,
0xDE, 0xEE, 0x7F, 0x38, 0xFD, 0x74, 0xB2, 0x5B, 0x64, 0xE3, 0xE5, 0x19, 0x6F, 0x2E, 0x46, 0x13,
0xF1, 0xC7, 0x6D, 0x2E, 0x75, 0xDA, 0xCF, 0xDF, 0x9E, 0xBE, 0xFE, 0xC3, 0xF6, 0x23, 0x46, 0x7A,
0x43, 0xF2, 0x30, 0xEA, 0x94, 0xCB, 0x0D, 0x26, 0x7A, 0xF3, 0x85, 0xDB, 0xBE, 0xFF, 0xCF, 0xE9,
0xFD, 0xDD, 0xE9, 0xCD, 0xAD, 0xDF, 0xF2, 0x76, 0xF7, 0xDD, 0x89, 0xEA, 0xDA, 0xB4, 0x71, 0x34,
0x3E, 0x38, 0xE2, 0xB0, 0x31, 0x6F, 0x77, 0x3F, 0x9C, 0xDE, 0x5A, 0x4B, 0x01, 0x3F, 0xD7, 0xFE,
0x8F, 0xF8, 0xF1, 0x74, 0x2F, 0xD7, 0x1E, 0xBF, 0x62, 0xAD, 0xED, 0xB4, 0x9B, 0x46, 0x0B, 0xEF,
0xB6, 0xBB, 0x7F, 0x9E, 0x3E, 0xDC, 0x9D, 0xFE, 0x82, 0xBF, 0x3F, 0x9D, 0x76, 0x89, 0x77, 0x2D,
0x98, 0x48, 0xAF, 0xC3, 0xE6, 0x8B, 0x29, 0x2F, 0xD6, 0x22, 0x3C, 0xD0, 0xC2, 0x31, 0x99, 0x04,
0x15, 0x5C, 0x55, 0xE1, 0xAA, 0x89, 0xF2, 0xE6, 0xAB, 0x7F, 0x7F, 0xF3, 0xE3, 0xDB, 0xB7, 0x6F,
0xAE, 0xEE, 0x7F, 0xF9, 0xEF, 0x37, 0x1F, 0xEF, 0xBE, 0xFD, 0xDF, 0x2F, 0xDB, 0xDB, 0xDB, 0x0F,
0xB7, 0xB7, 0xB9, 0xDC, 0xF0, 0xBB, 0xED, 0xDD, 0xBB, 0xED, 0xFD, 0xCD, 0x35, 0x86, 0xDF, 0x90,
0xAB, 0x92, 0x7F, 0x79, 0x7D, 0x96, 0x25, 0xBE, 0xBF, 0xF1, 0x13, 0xCB, 0xD5, 0xED, 0xFB, 0xC1,
0x02, 0x85, 0x9F, 0x34, 0xED, 0x22, 0x14, 0x7B, 0x10, 0xEF, 0xFE, 0xBE, 0x89, 0x7D, 0xC9, 0x5A,
0xFF, 0xDE, 0xDA, 0x90, 0xF0, 0xB3, 0xAB, 0xCD, 0xEC, 0xE6, 0x5C, 0x09, 0x26, 0x3B, 0xCE, 0x60,
0xB4, 0xD9, 0x38, 0xE7, 0xB6, 0x60, 0x3F, 0xAD, 0xA7, 0x73, 0xB7, 0x93, 0xF5, 0x70, 0xA3, 0xBF,
0xC6, 0xEF, 0x16, 0xBF, 0xDC, 0x5C, 0xC9, 0xFB, 0x8F, 0xE3, 0x4E, 0x0B, 0x57, 0x75, 0x8C, 0x6F,
0x30, 0x08, 0x78, 0xE6, 0xBD, 0xA8, 0x80, 0x9F, 0x9B, 0xE6, 0xE0, 0x59, 0xBD, 0x4A, 0x57, 0x5A,
0xE3, 0x0F, 0xDC, 0xE8, 0xB9, 0x8D, 0xC1, 0x4D, 0xB8, 0x96, 0x31, 0xDD, 0x14, 0x4F, 0xBA, 0xF8,
0x91, 0xF8, 0xC4, 0xC9, 0x50, 0x88, 0x20, 0x34, 0xC3, 0x7E, 0x1E, 0x5E, 0x7F, 0x2C, 0x67, 0xA0,
0x55, 0xCE, 0x5F, 0x05, 0xC0, 0x70, 0xBA, 0xF3, 0x7A, 0x01, 0x1C, 0x50, 0xB2, 0x00, 0x84, 0xC3,
0x04, 0x88, 0xFC, 0xF0, 0x29, 0xA0, 0x10, 0x02, 0x52, 0x88, 0x8D, 0x2A, 0x63, 0xE8, 0x51, 0xEF,
0x17, 0xF3, 0x3F, 0x62, 0x89, 0x64, 0x72, 0x28, 0x2E, 0xCF, 0x2C, 0xFC, 0x0C, 0x8B, 0x37, 0xB6,
0xB8, 0xF4, 0xF4, 0x2A, 0x55, 0x5D, 0x00, 0x43, 0x74, 0x53, 0x35, 0xF7, 0x4B, 0x66, 0xE4, 0x41,
0xE8, 0x02, 0x8D, 0x19, 0xCA, 0x27, 0xCF, 0x73, 0x96, 0x60, 0xE3, 0x98, 0x4B, 0xB2, 0x5E, 0x17,
0xDA, 0xEF, 0x9E, 0x9E, 0x6B, 0x37, 0x05, 0x44, 0xDF, 0x68, 0x4B, 0x3E, 0xCA, 0x5C, 0xDF, 0xE9,
0x2D, 0xC7, 0x02, 0x92, 0xDE, 0x72, 0x8A, 0xEB, 0x73, 0x4E, 0x09, 0x84, 0x98, 0x73, 0x1F, 0x51,
0xA2, 0x7F, 0x38, 0x43, 0x0A, 0x6E, 0x9D, 0x23, 0x29, 0x69, 0x62, 0x12, 0xCB, 0xFC, 0x79, 0x03,
0xB4, 0xBE, 0xBC, 0xFE, 0xD3, 0x0D, 0xC4, 0x7D, 0xF7, 0x4E, 0xA0, 0xF4, 0xE6, 0x36, 0x22, 0x15,
0x1A, 0x96, 0x2C, 0x4A, 0x8B, 0xD1, 0x0A, 0x54, 0x6E, 0xB6, 0x72, 0x1F, 0xD4, 0x56, 0xC0, 0x13,
0x7B, 0xCF, 0x20, 0x77, 0xFC, 0x58, 0xF7, 0x10, 0x85, 0xFB, 0x0A, 0x34, 0x56, 0x08, 0xCF, 0xAC,
0x80, 0x44, 0xCC, 0x21, 0x3A, 0xA4, 0xF2, 0x17, 0x05, 0x90, 0x4C, 0x9E, 0x91, 0xBF, 0x5D, 0xE7,
0xBE, 0x38, 0x9E, 0x1E, 0xCD, 0xB0, 0x84, 0xD7, 0x53, 0xCA, 0xF1, 0x61, 0xF3, 0xA5, 0x68, 0x88,
0x23, 0xE7, 0x47, 0x46, 0x93, 0xDC, 0x4A, 0x39, 0xE5, 0x34, 0x1E, 0x78, 0x3B, 0x5B, 0xB3, 0xC1,
0xD2, 0x47, 0xE3, 0x32, 0x12, 0x1C, 0x6F, 0x64, 0xA3, 0x33, 0x05, 0xC8, 0x47, 0xC9, 0x02, 0xA0,
0x43, 0xF1, 0xC4, 0x12, 0x88, 0x14, 0x93, 0xCF, 0xBF, 0x05, 0x95, 0x06, 0xA1, 0xE1, 0xB6, 0xF9,
0xDF, 0xBF, 0x7E, 0xB1, 0x3D, 0x26, 0xFE, 0xFC, 0x7D, 0xC7, 0x2F, 0xD6, 0xF5, 0xE4, 0x37, 0x78,
0x8E, 0x4C, 0xEB, 0x18, 0x3C, 0x39, 0xE3, 0x63, 0x09, 0x09, 0x73, 0x1B, 0x42, 0x74, 0xA1, 0x79,
0x00, 0x91, 0x55, 0x60, 0x16, 0xA2, 0x43, 0x8E, 0xCE, 0x36, 0x2A, 0x11, 0x23, 0x39, 0x6D, 0xF7,
0xE2, 0xF9, 0x60, 0x81, 0xE2, 0x2C, 0x13, 0x62, 0xAE, 0x24, 0x5C, 0x20, 0x26, 0x20, 0xBC, 0x64,
0xA7, 0xC4, 0xE0, 0x91, 0x4A, 0x30, 0x3B, 0x25, 0x0A, 0x89, 0x92, 0xD2, 0xF4, 0x5A, 0xF8, 0x9D,
0x3E, 0xF6, 0x4E, 0x86, 0x5A, 0x65, 0xCB, 0x32, 0x16, 0x13, 0xB1, 0xCD, 0x4A, 0x93, 0xF9, 0xB1,
0xBE, 0xD7, 0x35, 0x11, 0x37, 0x42, 0x13, 0x51, 0xBC, 0xB0, 0xFB, 0x2A, 0x1E, 0x29, 0x55, 0xA4,
0x86, 0xF8, 0x41, 0xF5, 0x90, 0x66, 0x01, 0xC4, 0x8D, 0xA2, 0xFE, 0x77, 0x7F, 0x62, 0x5B, 0xAF,
0x3E, 0xCA, 0x95, 0x41, 0x99, 0xF1, 0xCA, 0xB5, 0xB1, 0x4D, 0x06, 0x9E, 0x43, 0x86, 0x68, 0x73,
0x04, 0x25, 0x61, 0x40, 0xE2, 0xB1, 0x58, 0xD2, 0x09, 0xB2, 0xB1, 0x78, 0x94, 0x86, 0x58, 0x32,
0x32, 0x4B, 0x8E, 0x76, 0x7E, 0x12, 0x5F, 0x68, 0x99, 0xB3, 0x4E, 0xBF, 0x6B, 0xCA, 0xB6, 0x18,
0xB6, 0xCE, 0xE7, 0xDD, 0x26, 0x42, 0xF2, 0x85, 0x03, 0x0D, 0xDB, 0x55, 0x1A, 0xA4, 0x0F, 0xC3,
0xCA, 0xBE, 0xF1, 0xE6, 0x92, 0x95, 0xB7, 0x3B, 0x44, 0x68, 0xE2, 0xB8, 0x3C, 0x79, 0x8E, 0xA1,
0x8E, 0xF8, 0xB5, 0x72, 0xF4, 0xA7, 0x5E, 0x95, 0xDD, 0xDA, 0x24, 0x32, 0x73, 0x50, 0x85, 0x79,
0x2C, 0x46, 0x4A, 0xC3, 0xA8, 0xD8, 0x25, 0xED, 0x62, 0x45, 0xD5, 0xB8, 0x8E, 0xEC, 0xD2, 0x6B,
0x55, 0x72, 0xB4, 0xE8, 0x19, 0xF5, 0xAA, 0x19, 0xA9, 0x1B, 0x24, 0x29, 0x8B, 0xDB, 0x66, 0xC3,
0x49, 0xC3, 0xC9, 0x3E, 0x2E, 0x26, 0x4E, 0x22, 0x44, 0x33, 0xF1, 0xEE, 0x8B, 0xA4, 0x2A, 0x43,
0xA9, 0xC9, 0x5B, 0x49, 0x95, 0x52, 0x1A, 0xEE, 0xB2, 0x20, 0xA0, 0x5E, 0x7C, 0x3C, 0x2D, 0x6E,
0xEF, 0xE3, 0x66, 0x7C, 0xF4, 0xF9, 0x66, 0x24, 0xED, 0x2B, 0xCF, 0x98, 0xEB, 0x32, 0x2E, 0xE0,
0xEC, 0xCA, 0x2C, 0x40, 0x1E, 0x6A, 0x2F, 0x90, 0xEF, 0x06, 0x5A, 0x82, 0xA3, 0x5B, 0x72, 0x09,
0xA4, 0xDD, 0xE8, 0x4B, 0xD0, 0x75, 0xFF, 0xCC, 0xD1, 0xD9, 0xBC, 0xD8, 0x22, 0xF7, 0xFE, 0xF4,
0x0F, 0x54, 0x84, 0x97, 0x14, 0x3E, 0x64, 0x98, 0x8B, 0x52, 0x92, 0x8E, 0xCF, 0x26, 0xD9, 0x5C,
0xA4, 0x3A, 0xD9, 0x0C, 0xA5, 0xBC, 0xB3, 0x45, 0x92, 0x47, 0xA3, 0x07, 0x88, 0x65, 0x13, 0x00,
0x97, 0xC4, 0x2A, 0x50, 0xC2, 0x44, 0x8A, 0x84, 0x29, 0x41, 0x4D, 0x26, 0xE5, 0x08, 0x05, 0xA4,
0x75, 0x01, 0x9B, 0xCF, 0x24, 0xD4, 0x80, 0xB1, 0x8C, 0x08, 0x80, 0xDE, 0xC1, 0x19, 0xD4, 0xA4,
0x8C, 0x6C, 0x02, 0x3A, 0x3A, 0x8F, 0x00, 0xF9, 0x31, 0x1A, 0xDE, 0x74, 0xBE, 0x04, 0x91, 0x30,
0x61, 0x44, 0x20, 0x88, 0x8F, 0x67, 0x06, 0xD9, 0x37, 0xBB, 0x32, 0x53, 0xEF, 0x4F, 0xA1, 0x14,
0xE3, 0x62, 0x40, 0x8F, 0x34, 0x91, 0x3F, 0x9E, 0xA2, 0x85, 0xF9, 0x1C, 0xD9, 0x65, 0x74, 0xB4,
0x58, 0x03, 0x5B, 0x87, 0x30, 0x2F, 0x17, 0x2D, 0x04, 0x72, 0x21, 0x84, 0x55, 0xB6, 0x08, 0x34,
0x26, 0xEC, 0x30, 0xFC, 0xAC, 0x48, 0x04, 0x98, 0x23, 0xDC, 0x3F, 0xAB, 0x3C, 0xD1, 0x86, 0x79,
0x84, 0x7D, 0xA7, 0x06, 0x78, 0x37, 0x67, 0xE1, 0xDE, 0xA7, 0xCC, 0x80, 0x54, 0x29, 0x36, 0x2D,
0xCB, 0xE7, 0x62, 0x88, 0x9C, 0x57, 0xF6, 0x5D, 0xD2, 0x62, 0x0D, 0xF2, 0x6D, 0x12, 0x49, 0x77,
0x9D, 0x0A, 0xDA, 0x28, 0xD8, 0x28, 0x6E, 0xB3, 0xFE, 0x83, 0x28, 0xA6, 0x9D, 0xCC, 0x35, 0xE8,
0x12, 0x29, 0x93, 0x71, 0x07, 0x7D, 0x71, 0xC4, 0x58, 0x6F, 0x71, 0xDB, 0x2E, 0xDB, 0xE2, 0xE3,
0xA1, 0xC7, 0x82, 0x87, 0xA1, 0xF2, 0xD3, 0xE8, 0x11, 0x00, 0x7F, 0xFD, 0xB2, 0x6A, 0x67, 0x37,
0xC4, 0x97, 0xB6, 0x45, 0xA8, 0x72, 0xFB, 0xCD, 0x54, 0xDD, 0x6A, 0x62, 0xA7, 0x2C, 0x7D, 0x99,
0xB6, 0x4E, 0x88, 0x78, 0xE8, 0x35, 0xFF, 0x2B, 0x05, 0xF2, 0x11, 0x51, 0xA7, 0x48, 0xB5, 0x40,
0x9A, 0xD0, 0x4B, 0x64, 0xDB, 0x71, 0xD6, 0x12, 0x19, 0x5C, 0xC8, 0x51, 0x23, 0x3B, 0xE7, 0x8C,
0x0D, 0x88, 0x12, 0xA1, 0x9D, 0xAB, 0x65, 0x0F, 0xD9, 0x82, 0x8A, 0xD7, 0x34, 0x1D, 0xE1, 0xD6,
0x5C, 0x23, 0xBB, 0x60, 0x73, 0xE3, 0x7C, 0xAD, 0x9C, 0xEC, 0x28, 0x69, 0x89, 0x23, 0x58, 0x04,
0xFB, 0x5A, 0xCD, 0xFD, 0x39, 0x38, 0xCD, 0x2C, 0x41, 0x40, 0xAA, 0xC9, 0x9F, 0xEA, 0x75, 0xCD,
0x4C, 0xF2, 0x98, 0x34, 0xA5, 0x52, 0x65, 0x93, 0x24, 0x86, 0x89, 0x90, 0x44, 0x6A, 0x8D, 0xD4,
0x05, 0x84, 0x88, 0x45, 0xF1, 0xB7, 0x55, 0x23, 0xE4, 0xC2, 0xA2, 0x38, 0x10, 0x01, 0x7D, 0x2D,
0x7F, 0x2A, 0x37, 0x6D, 0x9A, 0x79, 0x42, 0x4E, 0x9D, 0xD8, 0xCA, 0x24, 0x2E, 0x46, 0xA1, 0xC4,
0x75, 0x2D, 0x95, 0x48, 0x0C, 0xD5, 0xA2, 0x26, 0x68, 0x32, 0x2E, 0x84, 0xED, 0xC6, 0x34, 0x97,
0x24, 0x6D, 0x6C, 0x35, 0x5D, 0x5A, 0x56, 0x15, 0x6A, 0x8A, 0xB9, 0xCD, 0x5B, 0x05, 0x94, 0x42,
0x80, 0xE4, 0x13, 0xC3, 0xA4, 0x89, 0x16, 0x0C, 0x24, 0x9B, 0x55, 0x69, 0xAD, 0x2D, 0xD6, 0x59,
0xDE, 0xCD, 0x23, 0x25, 0x08, 0x3B, 0xBC, 0xC2, 0x93, 0x19, 0xB5, 0x2C, 0x39, 0x17, 0x56, 0x8B,
0x6B, 0x01, 0x23, 0xD2, 0xA5, 0xBB, 0x73, 0x6A, 0x51, 0xEB, 0xE5, 0xB2, 0x7A, 0x51, 0x68, 0x61,
0x2A, 0x98, 0x7E, 0x14, 0x4C, 0xA8, 0x6C, 0xEB, 0xEC, 0x7D, 0x1E, 0xAF, 0x6A, 0x87, 0x65, 0xC5,
0xA0, 0x6A, 0xC7, 0xC6, 0xDD, 0x65, 0x0B, 0xAA, 0x76, 0x55, 0xAC, 0xAB, 0x10, 0x55, 0x6D, 0x5A,
0x94, 0x8D, 0x7A, 0xD5, 0x95, 0xED, 0x66, 0x41, 0x45, 0xB1, 0x5C, 0xF2, 0x36, 0x9B, 0x2F, 0xC9,
0xDC, 0x39, 0x2F, 0x86, 0x4E, 0x2A, 0x45, 0xE5, 0xDE, 0x5D, 0x92, 0x54, 0x5E, 0xB7, 0xCD, 0x6E,
0xAB, 0x47, 0x3C, 0xBD, 0x66, 0x26, 0x75, 0x31, 0xF5, 0x4B, 0xAD, 0x9B, 0x3B, 0x12, 0xFA, 0xD8,
0x19, 0x32, 0x7D, 0xCE, 0x05, 0x5D, 0xFB, 0xF2, 0x0B, 0x12, 0xBB, 0xA4, 0x0B, 0x66, 0xBB, 0x4A,
0x0B, 0xBE, 0x77, 0xED, 0x97, 0x50, 0xE8, 0x86, 0x5A, 0x82, 0xA6, 0x5B, 0xB4, 0x05, 0x18, 0xDB,
0xD5, 0xF8, 0x4B, 0x30, 0x76, 0x3F, 0xCD, 0x51, 0xDB, 0xFD, 0x39, 0x82, 0x5A, 0x92, 0xCF, 0x57,
0xAD, 0x7A, 0x06, 0x94, 0x86, 0x0C, 0xA1, 0xE2, 0x5C, 0x0B, 0x6B, 0x05, 0x0C, 0xFE, 0xD3, 0xB4,
0xE7, 0xCF, 0x26, 0x2C, 0xD6, 0xD8, 0xAA, 0xE9, 0x46, 0xAE, 0x27, 0x20, 0xD6, 0x66, 0x49, 0x68,
0x81, 0x8A, 0xAB, 0x0A, 0x66, 0x4B, 0x95, 0x86, 0x9A, 0x91, 0x93, 0xDA, 0xC2, 0x47, 0xC4, 0x83,
0x80, 0x01, 0x54, 0x0E, 0x25, 0x65, 0xA5, 0x16, 0x1F, 0x49, 0xBB, 0x42, 0xE4, 0xED, 0x14, 0x2A,
0x10, 0x41, 0xA7, 0x1A, 0x4C, 0x05, 0x9B, 0x9E, 0x6A, 0x73, 0x78, 0xBF, 0x45, 0x5D, 0x41, 0x75,
0x6A, 0x5E, 0x77, 0xD2, 0x01, 0x6A, 0x88, 0xD8, 0x12, 0xAA, 0x73, 0x53, 0x48, 0x5C, 0x5A, 0x08,
0x04, 0x35, 0x6D, 0x96, 0xA3, 0x82, 0x14, 0x1A, 0x88, 0x4B, 0x64, 0x57, 0xA9, 0x21, 0x69, 0x7B,
0x29, 0x05, 0x10, 0xB5, 0x42, 0x49, 0xE8, 0x33, 0x6D, 0x6D, 0x98, 0x9D, 0x15, 0xC3, 0x20, 0x51,
0xB6, 0xAB, 0x9A, 0x2C, 0xE4, 0x79, 0x54, 0x6A, 0xE5, 0xD1, 0x0E, 0x11, 0xD3, 0xB0, 0xD2, 0x30,
0xB5, 0x57, 0x92, 0xAC, 0x87, 0x61, 0x30, 0x70, 0x95, 0xA1, 0x06, 0x40, 0x95, 0x4C, 0xA8, 0x55,
0x5A, 0xA1, 0x41, 0x01, 0x14, 0x25, 0xA1, 0x89, 0x4E, 0xA2, 0x73, 0x50, 0x4D, 0x95, 0x24, 0xCA,
0xD7, 0x95, 0x43, 0x33, 0x89, 0xC7, 0x36, 0xB0, 0x9A, 0x09, 0x00, 0x53, 0xD3, 0x05, 0xA5, 0xC1,
0x9C, 0x59, 0x49, 0x62, 0xE3, 0x4A, 0x12, 0xBB, 0x3B, 0x65, 0x76, 0xCD, 0x1B, 0x42, 0xAD, 0x1E,
0x12, 0x9A, 0x78, 0xAD, 0x8E, 0x14, 0x85, 0x5D, 0xBB, 0x92, 0xF1, 0xDA, 0x11, 0x4A, 0xF2, 0x68,
0xCF, 0xD8, 0x60, 0xD3, 0x9C, 0x69, 0xF0, 0x4B, 0x98, 0xE2, 0x89, 0xD3, 0x9E, 0xB1, 0xAD, 0x54,
0x1B, 0x63, 0x42, 0x68, 0xB9, 0x46, 0x6D, 0x22, 0x49, 0x02, 0xF6, 0x8D, 0xD4, 0x25, 0x87, 0xAB,
0x59, 0x9B, 0xFC, 0x49, 0x47, 0x42, 0x7F, 0x20, 0x18, 0x9E, 0x8D, 0x21, 0x34, 0xF6, 0x1C, 0xC3,
0x64, 0x34, 0x48, 0xA2, 0xA7, 0x43, 0x34, 0xCC, 0xAB, 0xE1, 0x05, 0xAA, 0x67, 0x97, 0x7C, 0xF7,
0x03, 0xF2, 0x8F, 0xD0, 0xA4, 0x89, 0x2D, 0x93, 0xBF, 0x20, 0x3C, 0xBA, 0xE9, 0x18, 0xCB, 0xE2,
0x59, 0xA1, 0x62, 0xDB, 0xA0, 0x71, 0xDC, 0x31, 0x20, 0x34, 0xEB, 0x92, 0xE6, 0x81, 0x06, 0x15,
0x64, 0x00, 0xC4, 0xB6, 0x53, 0xD4, 0xEF, 0x98, 0x12, 0xAA, 0x2F, 0xB6, 0x25, 0xBC, 0xEA, 0x63,
0xA1, 0x41, 0xFF, 0xB6, 0xC1, 0x50, 0x90, 0x0A, 0xC9, 0x4A, 0x8B, 0xB1, 0x80, 0x99, 0xE4, 0xF0,
0x33, 0xB3, 0x77, 0x13, 0xEC, 0x85, 0x06, 0xFD, 0x29, 0x4E, 0xE1, 0x21, 0xB4, 0xE8, 0x3D, 0x2D,
0x71, 0x24, 0xC4, 0x80, 0x26, 0x7D, 0x8E, 0x38, 0xA5, 0x4D, 0x51, 0x39, 0xEE, 0xFB, 0xF8, 0x5A,
0x5A, 0xD4, 0xBF, 0x33, 0xBF, 0x6B, 0x3E, 0xEE, 0x0B, 0xD5, 0x02, 0x26, 0x3E, 0x6E, 0x8C, 0x22,
0x51, 0x52, 0xA2, 0xB8, 0x78, 0x9B, 0x05, 0x27, 0x75, 0xF1, 0xAA, 0x22, 0xA9, 0x8B, 0xB9, 0xD9,
0xA7, 0xDB, 0xC2, 0xAA, 0x8B, 0xFD, 0xD6, 0x6D, 0x56, 0xEB, 0x6E, 0x75, 0x71, 0xB7, 0x2E, 0x24,
0x94, 0x4D, 0x20, 0x12, 0x73, 0x69, 0xAE, 0x69, 0x6E, 0x90, 0x42, 0xA7, 0x2E, 0x9E, 0xDC, 0x25,
0x25, 0x4D, 0x5D, 0x3C, 0x39, 0x56, 0x02, 0x5A, 0xAF, 0x2A, 0x2C, 0x1A, 0x04, 0x24, 0xA4, 0xC1,
0x13, 0xB6, 0x09, 0x2A, 0xB2, 0xE5, 0xC3, 0xDC, 0x69, 0x06, 0x95, 0x96, 0x34, 0xF1, 0x71, 0x85,
0x64, 0x83, 0x9F, 0x4D, 0x2A, 0x6F, 0x85, 0x64, 0x83, 0x29, 0x68, 0xA2, 0x17, 0xCD, 0x80, 0x96,
0xAD, 0x21, 0x2C, 0x50, 0xCB, 0xED, 0x80, 0x3E, 0xE2, 0x5C, 0x6C, 0xC5, 0xDB, 0x14, 0x21, 0x92,
0x0F, 0xD4, 0xA4, 0x53, 0x2C, 0x81, 0xA6, 0x4E, 0xAE, 0x41, 0xD8, 0x82, 0x4E, 0x76, 0x96, 0xEA,
0xA4, 0x29, 0x30, 0x85, 0x56, 0x83, 0x56, 0xAF, 0x6A, 0x90, 0xB7, 0x80, 0xEE, 0xCF, 0x7A, 0xD0,
0x4F, 0xFC, 0x23, 0x3D, 0xD4, 0x85, 0x6C, 0x0D, 0xDA, 0x91, 0x47, 0x9A, 0x48, 0x7E, 0x9B, 0xF2,
0x4D, 0x15, 0x5D, 0x7B, 0x93, 0x9E, 0x98, 0x9A, 0x8E, 0xAE, 0x65, 0xB5, 0x96, 0xC1, 0xAA, 0x35,
0xB8, 0x66, 0xB5, 0x96, 0xE9, 0x86, 0xD5, 0xA6, 0x9C, 0xD8, 0xEC, 0x1B, 0x5A, 0x42, 0x6D, 0xC9,
0xB3, 0x79, 0xA2, 0x66, 0xCF, 0x96, 0x64, 0x87, 0xC7, 0xA6, 0x74, 0x3C, 0xF9, 0x76, 0x4A, 0xDC,
0x03, 0x05, 0x2D, 0xBF, 0xFB, 0x09, 0x2C, 0xB5, 0x10, 0x44, 0x15, 0x7C, 0x87, 0xD5, 0x5C, 0x32,
0x3A, 0x00, 0x7B, 0x61, 0xE1, 0x01, 0xD4, 0x34, 0x4A, 0x50, 0x1E, 0x90, 0x0E, 0xAD, 0x80, 0xF5,
0x62, 0x55, 0xC1, 0xEF, 0xB7, 0xB9, 0xA8, 0xB5, 0x18, 0x99, 0xAB, 0xDF, 0x88, 0xA5, 0xB9, 0x4C,
0xF6, 0xA0, 0x9B, 0xCB, 0x69, 0x0F, 0xCE, 0xBD, 0xDC, 0xBE, 0x7C, 0x1B, 0xFD, 0xDB, 0xCE, 0x8F,
0xD7, 0xB3, 0x48, 0x77, 0xE0, 0x2C, 0x52, 0x8E, 0xA0, 0xEA, 0x59, 0x6E, 0xA8, 0x27, 0xAB, 0x72,
0xEC, 0x4E, 0xC8, 0xD0, 0xF4, 0xFC, 0xB1, 0x24, 0xD7, 0x63, 0x49, 0x39, 0x42, 0x40, 0x4F, 0x68,
0xF9, 0xD8, 0xF1, 0xAF, 0x7F, 0x56, 0x2A, 0x8F, 0x96, 0xBB, 0x9E, 0xFD, 0xEF, 0xD2, 0xF5, 0x37,
0x77, 0x4F, 0xCD, 0x18, 0x8E, 0xE8, 0x99, 0xFA, 0xF9, 0x7E, 0xA8, 0x27, 0xB8, 0x00, 0x0E, 0xB6,
0xD8, 0xC9, 0xBB, 0xD7, 0xD2, 0x33, 0x3E, 0xAF, 0x67, 0x98, 0xC4, 0x6A, 0xAF, 0x1D, 0x00, 0x4E,
0x2F, 0x5B, 0x1D, 0x74, 0x72, 0xFB, 0xBB, 0x18, 0x7D, 0x82, 0x36, 0x99, 0xE4, 0x85, 0x56, 0xDC,
0x1F, 0xA4, 0xB3, 0x2C, 0xE7, 0x27, 0x4B, 0xE7, 0x25, 0x4E, 0x07, 0xEC, 0x18, 0x6C, 0x7F, 0x31,
0x17, 0xAE, 0xEB, 0x1A, 0x72, 0x90, 0x89, 0xBD, 0xA9, 0x2D, 0xAF, 0x65, 0xC8, 0x7C, 0xC0, 0x90,
0xA6, 0xBE, 0xCF, 0xDA, 0xA5, 0xFB, 0x0C, 0xCC, 0x59, 0x8E, 0x98, 0x13, 0xB5, 0xA8, 0xBD, 0xE7,
0x8C, 0x0D, 0x96, 0xD9, 0x21, 0xA3, 0xEC, 0x36, 0x0B, 0xF5, 0x0D, 0x96, 0xEC, 0x3B, 0x92, 0x8F,
0xB6, 0xEC, 0x2F, 0x69, 0x43, 0xE7, 0x40, 0xBA, 0x8E, 0x85, 0xC6, 0x03, 0xBA, 0xCE, 0x83, 0x25,
0xCB, 0x6B, 0xCC, 0xF8, 0x5A, 0x9E, 0xA1, 0xE7, 0x5F, 0x16, 0x25, 0x88, 0x27, 0x91, 0xBC, 0x2B,
0xDA, 0xDE, 0xC1, 0x5C, 0xE4, 0x99, 0xAB, 0xB3, 0x2C, 0xE7, 0x27, 0xBB, 0xFA, 0x15, 0xB9, 0xE9,
0x88, 0x6B, 0x42, 0xE9, 0x48, 0x3F, 0x63, 0x68, 0xEB, 0xAF, 0x9B, 0x39, 0x21, 0x2F, 0xA1, 0xA6,
0xEC, 0xAE, 0xA1, 0xEE, 0x01, 0xEB, 0xB3, 0xCB, 0x21, 0xFE, 0x0E, 0xBE, 0x79, 0xFE, 0x4D, 0x54,
0x92, 0x6D, 0x69, 0x8D, 0x9B, 0xA1, 0xEB, 0xE7, 0xE0, 0x9D, 0x23, 0x75, 0x4B, 0x4A, 0x74, 0xF3,
0x4E, 0x6A, 0x75, 0x8B, 0x09, 0xE5, 0x22, 0xED, 0xEF, 0xB0, 0xBD, 0xEF, 0x09, 0x2A, 0xA3, 0x4D,
0xB4, 0xD3, 0x93, 0xCF, 0xC1, 0x3D, 0xCF, 0x97, 0xC1, 0x94, 0xF4, 0x00, 0x61, 0x52, 0xB5, 0xBF,
0x9A, 0x9E, 0xDF, 0x03, 0xA6, 0xE8, 0xC1, 0x1C, 0xE5, 0x18, 0x02, 0x1D, 0x05, 0x9A, 0x21, 0x39,
0xF7, 0x47, 0x01, 0x97, 0x5E, 0x5F, 0xCF, 0xFA, 0x13, 0x67, 0x7D, 0x89, 0x39, 0xB5, 0x17, 0xD3,
0xE3, 0xBA, 0xD6, 0x18, 0x33, 0xCD, 0x86, 0xCE, 0x11, 0x80, 0x9F, 0x9A, 0x8B, 0x59, 0xFA, 0x51,
0x72, 0xFF, 0x96, 0xB7, 0x9B, 0x7F, 0xA9, 0x44, 0xBF, 0x4F, 0xB7, 0x73, 0x2B, 0x6F, 0xDA, 0x3C,
0x9A, 0x37, 0x3D, 0x93, 0x59, 0xBE, 0x6D, 0x09, 0xC6, 0x15, 0xCD, 0x3D, 0xA5, 0xA6, 0x9E, 0x1D,
0x0F, 0xE8, 0xDC, 0xA3, 0x1B, 0x9F, 0xBB, 0xB4, 0xEF, 0x1B, 0x04, 0x0D, 0x2C, 0x1D, 0x5D, 0x47,
0x69, 0x58, 0x3F, 0x37, 0x40, 0xD2, 0x78, 0x7A, 0xA1, 0x04, 0xD5, 0xEA, 0x47, 0x04, 0x7D, 0xA1,
0xAB, 0x95, 0xF1, 0x9C, 0x84, 0x19, 0x0D, 0xED, 0xCC, 0x47, 0x1F, 0x8E, 0xF1, 0x39, 0xD9, 0x21,
0xC5, 0x0C, 0x46, 0xF2, 0xB5, 0xDE, 0x35, 0x51, 0xD1, 0x40, 0x27, 0xF4, 0xD2, 0xA5, 0x23, 0xBD,
0x09, 0x82, 0x9D, 0x43, 0xCE, 0x68, 0x8E, 0x7B, 0x85, 0xF6, 0xB7, 0x97, 0x2A, 0xD6, 0xD7, 0xB9,
0x58, 0xB3, 0xCE, 0x78, 0x58, 0x35, 0xAE, 0x98, 0xC7, 0x5E, 0xB5, 0x2A, 0x16, 0x9A, 0x62, 0x4E,
0x5E, 0xB7, 0x95, 0x1E, 0x5B, 0x9E, 0x9F, 0x76, 0xD9, 0x0B, 0x34, 0x6B, 0x0B, 0x5D, 0xAE, 0x59,
0x63, 0x3C, 0xAA, 0xD9, 0x60, 0x94, 0x23, 0xC1, 0x4B, 0x18, 0x29, 0x62, 0xA3, 0x5B, 0xF4, 0xA0,
0xE9, 0x0C, 0xE7, 0x39, 0x2D, 0x25, 0x68, 0xB0, 0x4B, 0x92, 0x10, 0xC0, 0xAE, 0x4A, 0x38, 0xDD,
0x41, 0x48, 0x93, 0x7C, 0x17, 0x46, 0x8A, 0x30, 0x5B, 0x93, 0x0F, 0x8F, 0x35, 0x89, 0xF2, 0x26,
0xE7, 0x01, 0xCB, 0xF7, 0x23, 0x87, 0x84, 0x20, 0xAE, 0xF2, 0xBB, 0x70, 0x5C, 0xFE, 0x8A, 0x86,
0x56, 0x9E, 0x56, 0x21, 0xC8, 0xC5, 0x4D, 0xCE, 0x59, 0x8E, 0x49, 0x81, 0xAD, 0xA0, 0xEA, 0x93,
0xCA, 0xC5, 0x32, 0x34, 0xF1, 0x99, 0xE8, 0x81, 0x11, 0x55, 0x0A, 0x39, 0x73, 0x3D, 0x6C, 0x8B,
0x26, 0x05, 0x63, 0xAB, 0xF9, 0x94, 0x14, 0x67, 0x31, 0xE0, 0x9C, 0xD1, 0x0F, 0x20, 0x1B, 0x1F,
0xD9, 0x83, 0xD2, 0xFB, 0xD0, 0x3E, 0xEC, 0xEA, 0x9C, 0x7C, 0x39, 0x76, 0x38, 0xF1, 0x93, 0xE0,
0x39, 0x2F, 0x2C, 0xC8, 0xF5, 0x7B, 0xCD, 0xC6, 0x79, 0x5C, 0xDC, 0x58, 0xBB, 0xC5, 0xCE, 0xB8,
0x3A, 0x9C, 0xF5, 0xC4, 0x77, 0x35, 0xF5, 0x39, 0x21, 0xE4, 0x28, 0xA7, 0x45, 0x8D, 0xB7, 0xAF,
0x98, 0x23, 0x19, 0xCA, 0xCA, 0xC7, 0x74, 0xFB, 0x3A, 0x47, 0x33, 0x89, 0x1C, 0x4B, 0x2D, 0xF6,
0x7D, 0xAD, 0xFA, 0xC4, 0xA8, 0x84, 0x29, 0xCF, 0xFE, 0xD8, 0x45, 0xD4, 0x93, 0x61, 0x28, 0x2D,
0xEF, 0x6C, 0x8F, 0x98, 0x55, 0xCF, 0x3F, 0x54, 0x6A, 0xD7, 0x3E, 0xC6, 0x75, 0x74, 0x30, 0xB3,
0x3B, 0xE3, 0x59, 0xC1, 0xDF, 0x39, 0x6D, 0x39, 0x26, 0xBF, 0xE7, 0x26, 0xFE, 0x58, 0xF2, 0x20,
0x10, 0x83, 0x1C, 0xA3, 0xC5, 0x65, 0xC9, 0x9B, 0x63, 0x4B, 0xC6, 0xD4, 0xC1, 0x3F, 0xD6, 0x3C,
0x18, 0xA9, 0x49, 0xBF, 0x67, 0x99, 0xA5, 0xB5, 0x07, 0xF1, 0x80, 0x96, 0x6C, 0xE2, 0x3A, 0x5C,
0x1D, 0x10, 0xA7, 0xAB, 0xA4, 0x47, 0xCD, 0x4A, 0xCE, 0x3E, 0xF4, 0xE5, 0xC3, 0xE4, 0x70, 0x76,
0x51, 0xEF, 0x35, 0xB8, 0x26, 0xD6, 0xD7, 0x02, 0x2F, 0x65, 0xA4, 0x4C, 0x2B, 0x1F, 0x75, 0x8E,
0x95, 0x0E, 0xC7, 0x17, 0xB7, 0x2F, 0x48, 0x2F, 0xF5, 0xBE, 0x9C, 0xC4, 0xD2, 0x0B, 0xCC, 0x02,
0x78, 0x0F, 0x9E, 0x57, 0xCA, 0x36, 0x81, 0xCC, 0xE2, 0xB1, 0xA3, 0xC6, 0xE0, 0xC8, 0xED, 0xDB,
0xD8, 0x4B, 0x61, 0x39, 0x45, 0x7C, 0xF4, 0x4F, 0x86, 0x02, 0x9F, 0x8B, 0x78, 0xDF, 0xED, 0xDF,
0x18, 0x2D, 0x1F, 0x0C, 0xF8, 0xDC, 0x03, 0xBE, 0x31, 0x1E, 0x75, 0x40, 0x28, 0xFD, 0xE0, 0x70,
0x2C, 0x79, 0xD0, 0xE5, 0xC9, 0x6A, 0x87, 0x99, 0xCE, 0xAF, 0x79, 0x4E, 0x4B, 0xB2, 0xFD, 0xF0,
0x69, 0xAC, 0xE9, 0x8E, 0x86, 0x6E, 0x34, 0xEB, 0x9A, 0x87, 0x83, 0x3E, 0xF0, 0xBE, 0xDA, 0xD1,
0x70, 0x8F, 0xF4, 0x9C, 0x8A, 0x67, 0x97, 0x4B, 0x6C, 0x5A, 0xEA, 0xBE, 0xD0, 0xAC, 0x94, 0xD9,
0xBC, 0x6C, 0xC9, 0xD2, 0x36, 0xBF, 0x9D, 0xF1, 0x95, 0x36, 0x02, 0xEC, 0xA2, 0xEE, 0x04, 0xF2,
0x58, 0xE8, 0x70, 0x44, 0xF9, 0x64, 0xCA, 0x8A, 0xB5, 0xAB, 0x63, 0x6B, 0xE6, 0xFE, 0x65, 0xBB,
0x7C, 0xF6, 0x78, 0x49, 0x2F, 0x9A, 0xA3, 0xF1, 0xD9, 0xAB, 0x39, 0xB3, 0x7D, 0x50, 0x09, 0xD1,
0x91, 0x3B, 0x7D, 0x2B, 0xD6, 0xEB, 0xB8, 0x6C, 0xC7, 0xFF, 0x0F, 0x4D, 0xE8, 0x76, 0x0C
            };
        static string test = @"using System;

class FFT {
        readonly int SIZE;
        readonly int BITS;
        readonly double[] COS;
        readonly double[] SIN;
        public FFT(int n, bool inverse = false) {
            SIZE = n;
            BITS = (int)(Math.Log(SIZE) / Math.Log(2));
            COS = new double[SIZE / 2];
            SIN = new double[SIZE / 2];
            var dtheta = (inverse ? 2 : -2) * Math.PI / SIZE;
            for (int i = 0; i < COS.Length; i++) {
                COS[i] = Math.Cos(dtheta * i);
                SIN[i] = Math.Sin(dtheta * i);
            }
        }
        public void Exec(double[] real, double[] imag) {
            int j = 0;
            int n2 = SIZE / 2;
            for (int i = 1; i < SIZE - 1; i++) {
                int n1 = n2;
                while (j >= n1) {
                    j -= n1;
                    n1 /= 2;
                }
                j += n1;
                if (i < j) {
                    var t1 = real[i];
                    real[i] = real[j];
                    real[j] = t1;
                    t1 = imag[i];
                    imag[i] = imag[j];
                    imag[j] = t1;
                }
            }
            n2 = 1;
            for (int i = 0; i < BITS; i++) {
                int n1 = n2;
                n2 <<= 1;
                int w = 0;
                for (j = 0; j < n1; j++) {
                    var c = COS[w];
                    var s = SIN[w];
                    w += 1 << (BITS - i - 1);
                    for (int k = j; k < SIZE; k += n2) {
                        var t = k + n1;
                        var re = c * real[t] - s * imag[t];
                        var im = s * real[t] + c * imag[t];
                        real[k + n1] = real[k] - re;
                        imag[k + n1] = imag[k] - im;
                        real[k] += re;
                        imag[k] += im;
                    }
                }
            }
        }
    }
";

        static string dump(byte[] arr) {
            var str = "";
            var val = "";
            var chrLine = "";
            var m = 0;
            for (; m < arr.Length; m++) {
                var v = arr[m];
                val = v.ToString("X").PadLeft(2, '0');
                var addr = m.ToString("X").PadLeft(4, '0');
                var chr = (0x20 <= v && v <= 0x7F)
                    ? Encoding.ASCII.GetString(new byte[] { v })
                    : "ä"
                ;
                chrLine += chr;
                switch (m % 16) {
                case 0:
                    str += addr + "　" + val + " ";
                    break;
                case 3:
                case 7:
                case 11:
                    str += val + "　";
                    break;
                case 15:
                    str += val + "　" + chrLine + "\r\n";
                    chrLine = "";
                    break;
                default:
                    str += val + " ";
                    break;
                }
            }
            if (m % 16 != 15) {
                str += val + "　" + chrLine + "\r\n";
            }
            return str;
        }

        static void Main(string[] args) {
            var mm = test;
            for(int i=1; i<10; i++) {
                mm += test;
            }
            var arr = Encoding.ASCII.GetBytes(mm);

            var comp = Deflate.Compress(arr);
            var uncomp = Deflate.UnCompress(decData);

            var sw = new FileStream("C:\\Users\\user\\Desktop\\test.txt", FileMode.Create);
            sw.Write(uncomp, 0, uncomp.Length);
            sw.Flush();

            //Console.WriteLine(dump(uncomp));
            //Console.ReadKey();
        }
    }
}
