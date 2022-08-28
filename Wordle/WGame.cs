namespace Wordle;

internal class WGame
{
    public static string StandardLocation { get; } = Path.Join(AppDomain.CurrentDomain.BaseDirectory, "Words");
    public int WordLen { get; set; }
    public int WordCount { get; set; }
    public string[] Answers { get; init; }
    public string[] Guesses { get; init; }
    public string Word { get; private set; } = "";
    public int Tried { get; private set; }
    private static readonly Random _rng = new();

    public WGame(int wordLen, int wordCount, string[] answers, string[] guesses)
    {
        WordLen = wordLen;
        WordCount = wordCount;
        Answers = answers;
        Guesses = guesses;
    }

    public static WGame FromFiles(int wordLen, int wordCount, string answers, string guesses) => new(wordLen, wordCount, File.ReadAllLines(answers), File.ReadAllLines(guesses));
    public static WGame FromStandardLocation(int wordLen, int wordCount, string language)
    {
        var answer = Path.Join(StandardLocation, $"{language}-{wordLen}-answer.txt");
        var guess = Path.Join(StandardLocation, $"{language}-{wordLen}-guess.txt");

        if (!File.Exists(answer) || !File.Exists(guess))
            throw new ArgumentException("Given configuration doesn't exist");

        return FromFiles(wordLen, wordCount, answer, guess);
    }

    public void Start()
    {
        Tried = 0;
        Word = Answers[_rng.Next(Answers.Length)];
    }

    public GuessSummary Guess(string word)
    {
        if (Tried >= WordCount)
            return GuessSummary.JustReport(GuessReport.Error);
        if (word == Word)
        {
            Tried++;
            return new(word, GuessReport.Win, Array.Empty<LetterInfo>());
        }
        if (word.Length != WordLen)
            return GuessSummary.JustReport(GuessReport.InvalidLegth);
        if (!Answers.Contains(word) && !Guesses.Contains(word))
            return GuessSummary.JustReport(GuessReport.InvalidWord);
        Tried++;

        var lt = new LetterInfo[WordLen];
        for (int i = 0; i < WordLen; i++)
        {
            if (word[i] == Word[i])
                lt[i] = LetterInfo.Correct;
            else if (Word.Contains(word[i]))
                lt[i] = LetterInfo.Included;
            else
                lt[i] = LetterInfo.NotIncluded;
        }

        return new(word, Tried >= WordCount ? GuessReport.Lost : GuessReport.Valid, lt);
    }
}
