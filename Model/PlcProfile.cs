namespace Instrument_Plc_Converter.Model;

public class PlcProfile
{
    public string ProfileName { get; set; } = string.Empty;
    public double RawMin { get; set; }
    public double RawMax { get; set; }
}