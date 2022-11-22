using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

//------------------------
// Packet System by Thejuster
// MMORpgmaker Packet System
// (C) Copyright MakingItalia
//------------------------

namespace Packet
{

    public class PacketBase
    {

        public object PaketType(byte[] value)
        {
            byte[] data = new byte[100];
            object t = null;

            PacketData pdata = new PacketData();
            CharPaket cdata = new CharPaket();

            return t;
        }

    }


    /// <summary>
    /// Generic Packet Data  [ 0x0 ]
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct PacketData
    {
        public uint Command;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 24)]
        public String Argument1;


        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 24)]
        public String Argument2;

        public byte clienttype;


        public byte[] Serialize()
        {

            var buffer = new byte[Marshal.SizeOf(typeof(PacketData))];

            var gch = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            var pBuffer = gch.AddrOfPinnedObject();

            Marshal.StructureToPtr(this, pBuffer, false);
            gch.Free();

            return buffer;
        }


        public void Deserialize(ref byte[] data)
        {
            var gch = GCHandle.Alloc(data, GCHandleType.Pinned);
            this = (PacketData)Marshal.PtrToStructure(gch.AddrOfPinnedObject(), typeof(PacketData));
            gch.Free();
        }


    }


    /// <summary>
    /// Charaset Packet System  (Note:  60 Byte Totali ) [ 0x1 ]
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct CharPaket
    {
        public uint Command;

        public int accountID;

        public int CharNum;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 24)]
        public string CharName;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 24)]
        public string Class;

        public int level;

        public int exp;

        public int hp;
        public int sp;
        public int max_hp;
        public int max_sp;
        public int str;
        public int agi;
        public int vit;
        public int intel;
        public int dex;
        public int luk;
        public int hair_id;

        public byte[] Serialize()
        {

            var buffer = new byte[Marshal.SizeOf(typeof(CharPaket))];

            var gch = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            var pBuffer = gch.AddrOfPinnedObject();

            Marshal.StructureToPtr(this, pBuffer, false);
            gch.Free();

            return buffer;
        }

        public void Deserialize(ref byte[] data)
        {
            var gch = GCHandle.Alloc(data, GCHandleType.Pinned);
            this = (CharPaket)Marshal.PtrToStructure(gch.AddrOfPinnedObject(), typeof(CharPaket));
            gch.Free();
        }
    }




    public class PacketHeader
    {
        public enum HeaderCommand : uint
        {
            /// <summary>
            /// Request Login
            /// </summary>
            ACT_LOGIN = 0x41,
            /// <summary>
            /// Message
            /// </summary>
            ACT_MSG = 0x15,
            ACT_GET_P2LOC = 0x23,
            ACT_REQ_ROOM = 0x44,
            ACT_GET_ROOM = 0x45,
            /// <summary>
            /// Char Death
            /// </summary>
            ACT_CHAR_DEATH = 0x40,
            /// <summary>
            /// Request GET Account ID
            /// </summary>
            ACT_GET_ACC_ID = 0x16,
            /// <summary>
            /// Recive Account ID
            /// </summary>
            ACT_REC_ACC_ID = 0x17, 
            /// <summary>
            /// Get Char
            /// </summary>
            ACT_GET_CHAR = 0x18,
            /// <summary>
            /// Char is Empty
            /// </summary>
            CHAR_EMPTY = 0x19,
            /// <summary>
            /// Count Char per account
            /// </summary>
            ACT_COUNT_CHAR = 0x20,
            /// <summary>
            /// Request get Char Count
            /// </summary>
            ACT_GET_CHARCOUNT = 0x21,
            ACT_CREATE_CHAR = 0x22,
            ACT_CONFIRM = 0x50,
            ACT_FAIL = 0x51

        }


        public enum PacketType : byte
        {
            /// <summary>
            /// Packet Data
            /// </summary>
            PacketData = 0,
            /// <summary>
            /// Char Packet
            /// </summary>
            CharPacket = 1
        }
    }
}
