﻿using System;
using System.Collections.Generic;
using System.Numerics;

namespace SoulsFormats
{
    /// <summary>
    /// A graphics config file used in BB and DS3.
    /// </summary>
    public class GPARAM : SoulsFile<GPARAM>
    {
        /// <summary>
        /// Groups of params in this file.
        /// </summary>
        public List<Group> Groups;

        /// <summary>
        /// Unknown.
        /// </summary>
        public int Unk1, Unk2;

        /// <summary>
        /// Unknown.
        /// </summary>
        public byte[] UnkBlock1, UnkBlock2, UnkBlock3, UnkBlock4, UnkBlock5, CommentOffsetsBlock, CommentBlock;

        /// <summary>
        /// Read only.
        /// </summary>
        public List<string> Comments;

        /// <summary>
        /// Creates an uninitialized GPARAM. Should not be used publicly; use GPARAM.Read instead.
        /// </summary>
        public GPARAM() { }

        internal override bool Is(BinaryReaderEx br)
        {
            string magic = br.GetASCII(0, 8);
            return magic == "f\0i\0l\0t\0";
        }

        internal override void Read(BinaryReaderEx br)
        {
            br.BigEndian = false;

            // Don't @ me.
            br.AssertASCII("f\0i\0l\0t\0");
            br.AssertInt32(3);
            br.AssertInt32(0);
            int groupCount = br.ReadInt32();
            Unk1 = br.ReadInt32();
            // Header size
            br.AssertInt32(0x50);

            Offsets offsets;
            offsets.GroupHeaders = br.ReadInt32();
            offsets.ParamHeaderOffsets = br.ReadInt32();
            offsets.ParamHeaders = br.ReadInt32();
            offsets.Values = br.ReadInt32();
            offsets.Unk1 = br.ReadInt32();
            offsets.Unk2 = br.ReadInt32();

            Unk2 = br.ReadInt32();
            offsets.Unk3 = br.ReadInt32();
            offsets.Unk4 = br.ReadInt32();

            br.AssertInt32(0);
            offsets.Unk5 = br.ReadInt32();
            offsets.CommentOffsets = br.ReadInt32();
            offsets.Comments = br.ReadInt32();

            Groups = new List<Group>();
            for (int i = 0; i < groupCount; i++)
                Groups.Add(new Group(br, offsets));

            UnkBlock1 = br.GetBytes(offsets.Unk1, offsets.Unk2 - offsets.Unk1);
            UnkBlock2 = br.GetBytes(offsets.Unk2, offsets.Unk3 - offsets.Unk2);
            UnkBlock3 = br.GetBytes(offsets.Unk3, offsets.Unk4 - offsets.Unk3);
            UnkBlock4 = br.GetBytes(offsets.Unk4, offsets.Unk5 - offsets.Unk4);
            UnkBlock5 = br.GetBytes(offsets.Unk5, offsets.CommentOffsets - offsets.Unk5);
            CommentOffsetsBlock = br.GetBytes(offsets.CommentOffsets, offsets.Comments - offsets.CommentOffsets);
            CommentBlock = br.GetBytes(offsets.Comments, (int)br.Stream.Length - offsets.Comments);

            br.Position = offsets.Unk5;
            Comments = new List<string>();
            if (offsets.CommentOffsets < br.Stream.Length)
            {
                while (br.Position < offsets.Comments)
                {
                    int commentOffset = br.ReadInt32();
                    if (offsets.Comments + commentOffset < br.Stream.Length)
                    {
                        string comment = br.GetUTF16(offsets.Comments + commentOffset);
                        if (comment != "")
                            Comments.Add(comment);
                    }
                }
            }
        }

        internal override void Write(BinaryWriterEx bw)
        {
            bw.BigEndian = false;

            bw.WriteUTF16("filt");
            bw.WriteInt32(3);
            bw.WriteInt32(0);
            bw.WriteInt32(Groups.Count);
            bw.WriteInt32(Unk1);
            bw.WriteInt32(0x50);

            bw.ReserveInt32("GroupHeadersOffset");
            bw.ReserveInt32("ParamHeaderOffsetsOffset");
            bw.ReserveInt32("ParamHeadersOffset");
            bw.ReserveInt32("ValuesOffset");
            bw.ReserveInt32("UnkBlock1Offset");
            bw.ReserveInt32("UnkBlock2Offset");

            bw.WriteInt32(Unk2);
            bw.ReserveInt32("UnkBlock3Offset");
            bw.ReserveInt32("UnkBlock4Offset");

            bw.WriteInt32(0);
            bw.ReserveInt32("UnkBlock5Offset");
            bw.ReserveInt32("CommentOffsetsOffset");
            bw.ReserveInt32("CommentsOffset");

            for (int i = 0; i < Groups.Count; i++)
                Groups[i].WriteHeaderOffset(bw, i);

            int groupHeadersOffset = (int)bw.Position;
            bw.FillInt32("GroupHeadersOffset", groupHeadersOffset);
            for (int i = 0; i < Groups.Count; i++)
                Groups[i].WriteHeader(bw, i, groupHeadersOffset);

            int paramHeaderOffsetsOffset = (int)bw.Position;
            bw.FillInt32("ParamHeaderOffsetsOffset", paramHeaderOffsetsOffset);
            for (int i = 0; i < Groups.Count; i++)
                Groups[i].WriteParamHeaderOffsets(bw, i, paramHeaderOffsetsOffset);

            int paramHeadersOffset = (int)bw.Position;
            bw.FillInt32("ParamHeadersOffset", paramHeadersOffset);
            for (int i = 0; i < Groups.Count; i++)
                Groups[i].WriteParamHeaders(bw, i, paramHeadersOffset);

            int valuesOffset = (int)bw.Position;
            bw.FillInt32("ValuesOffset", valuesOffset);
            for (int i = 0; i < Groups.Count; i++)
                Groups[i].WriteValues(bw, i, valuesOffset);

            bw.FillInt32("UnkBlock1Offset", (int)bw.Position);
            bw.WriteBytes(UnkBlock1);

            bw.FillInt32("UnkBlock2Offset", (int)bw.Position);
            bw.WriteBytes(UnkBlock2);

            bw.FillInt32("UnkBlock3Offset", (int)bw.Position);
            bw.WriteBytes(UnkBlock3);

            bw.FillInt32("UnkBlock4Offset", (int)bw.Position);
            bw.WriteBytes(UnkBlock4);

            bw.FillInt32("UnkBlock5Offset", (int)bw.Position);
            bw.WriteBytes(UnkBlock5);

            bw.FillInt32("CommentOffsetsOffset", (int)bw.Position);
            bw.WriteBytes(CommentOffsetsBlock);

            bw.FillInt32("CommentsOffset", (int)bw.Position);
            bw.WriteBytes(CommentBlock);
        }

        /// <summary>
        /// Returns the first group with a matching name, or null if not found.
        /// </summary>
        public Group this[string name1]
        {
            get
            {
                foreach (Group group in Groups)
                {
                    if (group.Name1 == name1)
                        return group;
                }
                return null;
            }
        }

        internal struct Offsets
        {
            public int GroupHeaders;
            public int ParamHeaderOffsets;
            public int ParamHeaders;
            public int Values;
            public int Unk1;
            public int Unk2;
            public int Unk3;
            public int Unk4;
            public int Unk5;
            public int CommentOffsets;
            public int Comments;
        }

        /// <summary>
        /// A group of graphics params.
        /// </summary>
        public class Group
        {
            /// <summary>
            /// Identifies the group.
            /// </summary>
            public string Name1;

            /// <summary>
            /// Identifies the group, but shorter?
            /// </summary>
            public string Name2;

            /// <summary>
            /// Params in this group.
            /// </summary>
            public List<Param> Params;

            internal Group(BinaryReaderEx br, Offsets offsets)
            {
                int groupHeaderOffset = br.ReadInt32();
                br.StepIn(offsets.GroupHeaders + groupHeaderOffset);

                int paramCount = br.ReadInt32();
                int paramHeaderOffsetsOffset = br.ReadInt32();
                Name1 = br.ReadUTF16();
                Name2 = br.ReadUTF16();

                br.StepIn(offsets.ParamHeaderOffsets + paramHeaderOffsetsOffset);
                Params = new List<Param>();
                for (int i = 0; i < paramCount; i++)
                    Params.Add(new Param(br, offsets));
                br.StepOut();

                br.StepOut();
            }

            internal void WriteHeaderOffset(BinaryWriterEx bw, int groupIndex)
            {
                bw.ReserveInt32($"GroupHeaderOffset{groupIndex}");
            }

            internal void WriteHeader(BinaryWriterEx bw, int groupIndex, int groupHeadersOffset)
            {
                bw.FillInt32($"GroupHeaderOffset{groupIndex}", (int)bw.Position - groupHeadersOffset);
                bw.WriteInt32(Params.Count);
                bw.ReserveInt32($"ParamHeaderOffsetsOffset{groupIndex}");
                bw.WriteUTF16(Name1, true);
                bw.WriteUTF16(Name2, true);
                bw.Pad(4);
            }

            internal void WriteParamHeaderOffsets(BinaryWriterEx bw, int groupIndex, int paramHeaderOffsetsOffset)
            {
                bw.FillInt32($"ParamHeaderOffsetsOffset{groupIndex}", (int)bw.Position - paramHeaderOffsetsOffset);
                for (int i = 0; i < Params.Count; i++)
                    Params[i].WriteParamHeaderOffset(bw, groupIndex, i);
            }

            internal void WriteParamHeaders(BinaryWriterEx bw, int groupindex, int paramHeadersOffset)
            {
                for (int i = 0; i < Params.Count; i++)
                    Params[i].WriteParamHeader(bw, groupindex, i, paramHeadersOffset);
            }

            internal void WriteValues(BinaryWriterEx bw, int groupindex, int valuesOffset)
            {
                for (int i = 0; i < Params.Count; i++)
                    Params[i].WriteValues(bw, groupindex, i, valuesOffset);
            }

            /// <summary>
            /// Returns the first param with a matching name, or null if not found.
            /// </summary>
            public Param this[string name1]
            {
                get
                {
                    foreach (Param param in Params)
                    {
                        if (param.Name1 == name1)
                            return param;
                    }
                    return null;
                }
            }

            /// <summary>
            /// Returns the long and short names of the group.
            /// </summary>
            public override string ToString()
            {
                return $"{Name1} | {Name2}";
            }
        }

        /// <summary>
        /// Value types allowed in a param.
        /// </summary>
        public enum ParamType : byte
        {
            /// <summary>
            /// One byte.
            /// </summary>
            Unk1 = 0x1,

            /// <summary>
            /// One short.
            /// </summary>
            Unk2 = 0x2,

            /// <summary>
            /// One int.
            /// </summary>
            Unk3 = 0x3,

            /// <summary>
            /// One bool.
            /// </summary>
            Unk5 = 0x5,

            /// <summary>
            /// One int.
            /// </summary>
            Unk7 = 0x7,

            /// <summary>
            /// One float.
            /// </summary>
            Unk9 = 0x9,

            /// <summary>
            /// One bool.
            /// </summary>
            UnkB = 0xB,

            /// <summary>
            /// Two floats and 8 unused bytes.
            /// </summary>
            UnkC = 0xC,

            /// <summary>
            /// Four floats.
            /// </summary>
            UnkE = 0xE,

            /// <summary>
            /// Four bytes?
            /// </summary>
            UnkF = 0xF,
        }

        /// <summary>
        /// 
        /// </summary>
        public class Param
        {
            /// <summary>
            /// Identifies the param specifically.
            /// </summary>
            public string Name1;

            /// <summary>
            /// Identifies the param generically.
            /// </summary>
            public string Name2;

            /// <summary>
            /// Type of values in this param.
            /// </summary>
            public ParamType Type;

            /// <summary>
            /// Values in this param.
            /// </summary>
            public List<object> Values;

            /// <summary>
            /// Offset to something in the next block. Don't change this.
            /// </summary>
            public int UnkOffset1;

            internal Param(BinaryReaderEx br, Offsets offsets)
            {
                int paramHeaderOffset = br.ReadInt32();
                br.StepIn(offsets.ParamHeaders + paramHeaderOffset);

                int valuesOffset = br.ReadInt32();
                UnkOffset1 = br.ReadInt32();

                Type = br.ReadEnum8<ParamType>();
                byte valueCount = br.ReadByte();
                br.AssertByte(0);
                br.AssertByte(0);

                Name1 = br.ReadUTF16();
                Name2 = br.ReadUTF16();

                br.StepIn(offsets.Values + valuesOffset);
                Values = new List<object>();
                for (int i = 0; i < valueCount; i++)
                {
                    switch (Type)
                    {
                        case ParamType.Unk1:
                            Values.Add(br.ReadInt32());
                            break;

                        case ParamType.Unk2:
                            Values.Add(br.ReadInt16());
                            break;

                        case ParamType.Unk3:
                            Values.Add(br.ReadInt32());
                            break;

                        case ParamType.Unk5:
                            Values.Add(br.ReadBoolean());
                            break;

                        case ParamType.Unk7:
                            Values.Add(br.ReadInt32());
                            break;

                        case ParamType.Unk9:
                            Values.Add(br.ReadSingle());
                            break;

                        case ParamType.UnkB:
                            Values.Add(br.ReadBoolean());
                            break;

                        case ParamType.UnkC:
                            Values.Add(br.ReadVector2());
                            br.AssertInt32(0);
                            br.AssertInt32(0);
                            break;

                        case ParamType.UnkE:
                            Values.Add(br.ReadVector4());
                            break;

                        case ParamType.UnkF:
                            Values.Add(br.ReadInt32());
                            break;
                    }
                }
                br.StepOut();

                br.StepOut();
            }

            internal void WriteParamHeaderOffset(BinaryWriterEx bw, int groupIndex, int paramIndex)
            {
                bw.ReserveInt32($"ParamHeaderOffset{groupIndex}:{paramIndex}");
            }

            internal void WriteParamHeader(BinaryWriterEx bw, int groupIndex, int paramIndex, int paramHeadersOffset)
            {
                bw.FillInt32($"ParamHeaderOffset{groupIndex}:{paramIndex}", (int)bw.Position - paramHeadersOffset);
                bw.ReserveInt32($"ValuesOffset{groupIndex}:{paramIndex}");
                bw.WriteInt32(UnkOffset1);

                bw.WriteByte((byte)Type);
                bw.WriteByte((byte)Values.Count);
                bw.WriteByte(0);
                bw.WriteByte(0);

                bw.WriteUTF16(Name1, true);
                bw.WriteUTF16(Name2, true);
                bw.Pad(4);
            }

            internal void WriteValues(BinaryWriterEx bw, int groupIndex, int paramIndex, int valuesOffset)
            {
                bw.FillInt32($"ValuesOffset{groupIndex}:{paramIndex}", (int)bw.Position - valuesOffset);
                for (int i = 0; i < Values.Count; i++)
                {
                    object value = Values[i];
                    switch (Type)
                    {
                        case ParamType.Unk1:
                            bw.WriteInt32((int)value);
                            break;

                        case ParamType.Unk2:
                            bw.WriteInt16((short)value);
                            break;

                        case ParamType.Unk3:
                            bw.WriteInt32((int)value);
                            break;

                        case ParamType.Unk5:
                            bw.WriteBoolean((bool)value);
                            break;

                        case ParamType.Unk7:
                            bw.WriteInt32((int)value);
                            break;

                        case ParamType.Unk9:
                            bw.WriteSingle((float)value);
                            break;

                        case ParamType.UnkB:
                            bw.WriteBoolean((bool)value);
                            break;

                        case ParamType.UnkC:
                            bw.WriteVector2((Vector2)value);
                            bw.WriteInt32(0);
                            bw.WriteInt32(0);
                            break;

                        case ParamType.UnkE:
                            bw.WriteVector4((Vector4)value);
                            break;

                        case ParamType.UnkF:
                            bw.WriteInt32((int)value);
                            break;
                    }
                }
                bw.Pad(4);
            }

            /// <summary>
            /// Returns the value in this param at the given index.
            /// </summary>
            public object this[int index]
            {
                get
                {
                    return Values[index];
                }
                set
                {
                    Values[index] = value;
                }
            }

            /// <summary>
            /// Returns the specific and generic names of the param.
            /// </summary>
            public override string ToString()
            {
                return $"{Name1} | {Name2}";
            }
        }
    }
}
