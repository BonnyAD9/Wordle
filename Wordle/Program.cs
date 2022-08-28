using Bny.Console;

namespace Wordle;

public class Program
{
    public static void Main(string[] args)
    {
        WGame game = WGame.FromStandardLocation(5, 6, "en");
        
        while (true)
        {
            //Term.Erase();
            game.Start();
            //Term.FormLine("=====", Term.black, game.Word, Term.reset);
            Console.WriteLine("=====");
            while (game.Tried < game.WordCount)
            {
                var res = game.Guess(Term.Read("abcdefghijklmnopqrstuvwxyz", max: 5, next: ""));
                if (res.Report is GuessReport.InvalidLegth or GuessReport.Error or GuessReport.InvalidWord)
                {
                    Term.Form(Term.column, 0, Term.eraseLine);
                    continue;
                }
                Term.FormLine(Term.column, 0, res.GetColored());
                if (res.Report == GuessReport.Lost)
                    Term.FormLine(Term.brightRed, game.Word);
                Term.ResetColor();
                if (res.Report == GuessReport.Win)
                    break;
            }
        }
    }
}