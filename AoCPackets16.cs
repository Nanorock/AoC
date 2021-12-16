using System.Text;

namespace AdventOfCode_2021;

class AoCPackets16 : AdventOfCode
{
    Packet _inputPacket;
    public override void Run1()
    {
        var stream = BitStream.FromHex(inputFile[0]);
        _inputPacket = new Packet(stream);
        WriteLine($"Packet Version sum is {_inputPacket.SumVersion()}");
    }
    public override void Run2()
    {
        WriteLine($"Packet decoded value is {_inputPacket.Decode()}");
    }
}

class BitStream
{
    public static BitStream FromHex(string hex) => new BitStream(hex, true);
    public static BitStream FromBinary(string binaryStr) => new BitStream(binaryStr, false);

    readonly string _binaryString;
    readonly StringBuilder _sb = new StringBuilder();

    int _cursor = -1;

    BitStream(string input, bool fromHexa)
    {
        _binaryString = input;
        if (!fromHexa) return;
        for (int i = 0; i < input.Length; i++)
            _sb.Append(HexToByteString(input[i]));
        for (int i = _sb.Length - 1; i >= 0; i--) //Remove trailing zeros
        {
            if (_sb[i] != '0')
            {
                _sb.Length = i + 1;
                break;
            }
        }
        _binaryString = _sb.ToString();
        _sb.Clear();
    }

    public bool ReadNext(out int number, int count)
    {
        if (!ReadNext(out string str, count))
        {
            number = 0;
            return false;
        }
        number = Convert.ToInt32(str, 2);
        return true;
    }
    public bool ReadNext(out string str, int count)
    {
        _sb.Clear();
        for (int i = 0; i < count; i++)
        {
            if (ReadNext(out var c))
                _sb.Append(c);
            else
            {
                str = _sb.ToString();
                return false;
            }
        }
        str = _sb.ToString();
        return true;
    }
    public bool ReadNext(out char c)
    {
        c = ' ';
        if (++_cursor >= _binaryString.Length)
            return false;
        c = _binaryString[_cursor];
        return true;
    }
    
    public bool IsFinished => _cursor >= _binaryString.Length-1;

    //public string Read()
    static string HexToByteString(char c)
    {
        switch(c)
        {
            case '0':return"0000";
            case '1':return"0001";
            case '2':return"0010";
            case '3':return"0011";
            case '4':return"0100";
            case '5':return"0101";
            case '6':return"0110";
            case '7':return"0111";
            case '8':return"1000";
            case '9':return"1001";
            case 'A':return"1010";
            case 'B':return"1011";
            case 'C':return"1100";
            case 'D':return"1101";
            case 'E':return"1110";
            case 'F':return"1111";
        }
        return "";
    }
}

class Packet
{
    readonly int _version;
    readonly int _type;
    readonly ulong _value;

    public Packet(BitStream reader)
    {
        reader.ReadNext(out _version, 3);
        reader.ReadNext(out _type, 3);

        if (_type == 4)
            _value = ParseValue(reader);
        else
            ParsePackets(reader);
    }

    static StringBuilder _sb = new StringBuilder();
    ulong ParseValue(BitStream reader)
    {
        _sb.Clear();
        while (reader.ReadNext(out string str, 5))
        {
            _sb.Append(str.Substring(1, str.Length - 1));
            if (str[0] == '0')
                break;
        }
        return Convert.ToUInt64(_sb.ToString(), 2);
    }

    readonly List<Packet> _subPackets = new List<Packet>();
    enum PacketMode { PacketLength, PacketCount }
    void ParsePackets(BitStream reader)
    {
        reader.ReadNext(out char c);
        var mode = c == '0' ? PacketMode.PacketLength : PacketMode.PacketCount;
        if (mode == PacketMode.PacketLength)
        {
            reader.ReadNext(out int subPacketLength, 15);
            reader.ReadNext(out string content, subPacketLength);
            var subStream = BitStream.FromBinary(content);
            while (!subStream.IsFinished)
                AddPacket(subStream);
        }
        else //if (mode == PacketMode.PacketCount)
        {
            reader.ReadNext(out int subPacketCount, 11);
            for (int i = 0; i < subPacketCount; i++)
                AddPacket(reader);
        }
    }
    void AddPacket(BitStream reader) => _subPackets.Add(new Packet(reader));

    public int SumVersion() => _version + _subPackets.Sum(t => t.SumVersion());
    public ulong Decode()
    {
        return _type switch
        {
            0 => _subPackets.Aggregate<Packet, ulong>(0, (current, t) => current + t.Decode()),
            1 => _subPackets.Aggregate<Packet, ulong>(1, (current, t) => current * t.Decode()),
            2 => _subPackets.Min(p => p.Decode()),
            3 => _subPackets.Max(p => p.Decode()),
            5 => _subPackets[0].Decode() > _subPackets[1].Decode() ? 1ul : 0ul,
            6 => _subPackets[0].Decode() < _subPackets[1].Decode() ? 1ul : 0ul,
            7 => _subPackets[0].Decode() == _subPackets[1].Decode() ? 1ul : 0ul,
            _ => _value,
        };
    }
    public override string ToString() => _type switch { 4 => _value.ToString(), 0 => "sum", 1 => "product", 2 => "max", 3 => "min", 5 => ">", 6 => "<", _ => "==" };
}
