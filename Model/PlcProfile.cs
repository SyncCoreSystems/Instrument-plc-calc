namespace Instrument_Plc_Converter.Model;

public class PlcProfile
{
    public string ProfileName { get; set; } = string.Empty;
    public int RawMin { get; set; }
    public int RawMax { get; set; }
}