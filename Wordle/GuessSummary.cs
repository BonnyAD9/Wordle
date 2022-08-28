using Bny.Console;
using System.Text;

namespace Wordle;

internal record GuessSummary(string Word, GuessReport Report, LetterInfo[] Letters)
{
    public static GuessSummary JustReport(GuessReport report) => new("", report, Array.Empty<LetterInfo>());
    public string GetColored()
    {
        if (Word == "")
            return "";
        if (Report == GuessReport.Win)
            return Term.brightGreen + Word;
        
        StringBuilder sb = new();
        for (int i = 0; i < Letters.Length; i++)
        {
            sb.Append(Letters[i] switch
            {
                LetterInfo.Correct => Term.brightGreen,
                LetterInfo.Included => Term.brightCyan,
                _ => Term.white,
            }).Append(Word[i]);
        }

        return sb.ToString();
    }
}