using System.Text;

namespace RedisCS;

public class RESPParser
{
    
    private StringBuilder workedMessage {get;set;}
    private char[] _starters = ['+', '-', '*', ':', '$'];
    private string _ender = @"\r\n";
    public bool Enqueue(string signal)
    {
        workedMessage.Append(signal);
        // algorithm: on receiving new signal, look for starter or ender
        // if
    }    
}