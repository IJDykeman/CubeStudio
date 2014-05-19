﻿using System;
using System.Threading;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace CubePainter
{
    static class ColorPallete
    {
        public static Color[] colorArray;

        


        public static void loadContent()
        {
            String[] colorStringArray = {
                                  #region colors
"0 0 0",
"37 37 37",
"52 52 52",
"78 78 78",
"104 104 104",
"117 117 117",
"142 142 142",
"164 164 164",
"184 184 184",
"197 197 197",
"208 208 208",
"215 215 215",
"225 225 225",
"234 234 234",
"244 244 244",
"255 255 255",
"65 32 0",
"84 40 0",
"118 55 0",
"154 80 0",
"195 104 6",
"228 123 7",
"255 145 26",
"255 171 29",
"255 197 31",
"255 208 59",
"255 216 76",
"255 230 81",
"255 244 86",
"255 249 112",
"255 255 144",
"255 255 170",
"69 25 4",
"114 30 17",
"159 36 30",
"179 58 32",
"200 81 32",
"227 105 32",
"252 129 32",
"253 140 37",
"254 152 44",
"255 174 56",
"255 185 70",
"255 191 81",
"255 198 109",
"255 213 135",
"255 228 152",
"255 230 171",
"93 31 12",
"122 36 13",
"152 44 14",
"176 47 15",
"191 54 36",
"211 78 42",
"231 98 62",
"243 110 74",
"253 120 84",
"255 138 106",
"255 152 124",
"255 164 139",
"255 179 158",
"255 194 178",
"255 208 195",
"255 218 208",
"74 23 0",
"114 31 0",
"168 19 0",
"200 33 10",
"223 37 18",
"236 59 36",
"250 82 54",
"252 97 72",
"255 112 95",
"255 126 126",
"255 143 143",
"255 157 158",
"255 171 173",
"255 185 189",
"255 199 206",
"255 202 222",
"73 0 54",
"102 0 75",
"128 3 95",
"149 15 116",
"170 34 136",
"186 61 153",
"202 77 169",
"215 90 182",
"228 103 195",
"239 114 206",
"251 126 218",
"255 141 225",
"255 157 229",
"255 165 231",
"255 175 234",
"255 184 236",
"72 3 108",
"92 4 136",
"101 13 144",
"123 35 167",
"147 59 191",
"157 69 201",
"167 79 211",
"178 90 222",
"189 101 233",
"197 109 241",
"206 118 250",
"213 131 255",
"218 144 255",
"222 156 255",
"226 169 255",
"230 182 255",
"5 30 129",
"6 38 165",
"8 47 202",
"38 61 212",
"68 76 222",
"79 90 236",
"90 104 255",
"101 117 255",
"113 131 255",
"128 145 255",
"144 160 255",
"151 169 255",
"159 178 255",
"175 190 255",
"192 203 255",
"205 211 255",
"11 7 121",
"32 28 142",
"53 49 163",
"70 66 180",
"87 83 197",
"97 93 207",
"109 105 219",
"123 119 233",
"137 133 247",
"145 141 255",
"156 152 255",
"167 164 255",
"178 175 255",
"187 184 255",
"195 193 255",
"211 209 255",
"29 41 90",
"29 56 118",
"29 72 146",
"29 92 172",
"29 113 198",
"50 134 207",
"72 155 217",
"78 168 236",
"85 182 255",
"105 202 255",
"116 203 255",
"130 211 255",
"141 218 255",
"159 212 255",
"180 226 255",
"192 235 255",
"0 75 89",
"0 93 110",
"0 111 132",
"0 132 156",
"0 153 191",
"0 171 202",
"0 188 222",
"0 208 245",
"16 220 255",
"62 225 255",
"100 231 255",
"118 234 255",
"139 237 255",
"154 239 255",
"177 243 255",
"199 246 255",
"0 72 0",
"0 84 0",
"3 107 3",
"14 118 14",
"24 128 24",
"39 146 39",
"54 164 54",
"78 185 78",
"81 205 81",
"114 218 114",
"124 228 124",
"133 237 133",
"153 242 153",
"179 247 179",
"195 249 195",
"205 252 205",
"22 64 0",
"28 83 0",
"35 102 0",
"40 120 0",
"46 140 0",
"58 152 12",
"71 165 25",
"81 175 35",
"92 186 46",
"113 207 67",
"133 227 87",
"141 235 95",
"151 245 105",
"160 254 114",
"177 255 138",
"188 255 154",
"44 53 0",
"56 68 0",
"68 82 0",
"73 86 0",
"96 113 0",
"108 127 0",
"121 141 10",
"139 159 28",
"158 178 47",
"171 191 60",
"184 204 73",
"194 214 83",
"205 225 83",
"219 239 108",
"232 252 121",
"242 255 171",
"70 58 9",
"77 63 9",
"84 69 9",
"108 88 9",
"144 118 9",
"171 139 10",
"193 161 32",
"208 176 47",
"222 190 61",
"230 198 69",
"237 205 76",
"245 216 98",
"251 226 118",
"252 238 152",
"253 243 169",
"253 243 190",
"64 26 2",
"88 31 5",
"112 36 8",
"141 58 19",
"171 81 31",
"181 100 39",
"191 119 48",
"208 133 58",
"225 147 68",
"237 160 78",
"249 173 88",
"252 183 92",
"255 193 96",
"255 202 105",
"255 207 126",
"255 218 150",

        #endregion
                              };
            colorArray = new Color[256];
            for(int i=0;i<colorStringArray.Length;i++){
                
                String[] array = colorStringArray[i].Split(' ');
                colorArray[i] = new Color(Convert.ToInt32(array[0]), Convert.ToInt32(array[1]), Convert.ToInt32(array[2]));

            }

        }
    }
}
