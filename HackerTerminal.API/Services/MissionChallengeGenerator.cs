using System.Security.Cryptography;
using System.Text;

namespace HackerTerminal.API.Services;

public static class MissionChallengeGenerator
{
    public static bool TryGenerate(
        string? category,
        out string prompt,
        out string answer)
    {
        switch (category?.Trim().ToUpperInvariant())
        {
            case "PASSWORD":
                (prompt, answer) = GeneratePasswordChallenge();
                return true;

            case "CIPHER":
                (prompt, answer) = GenerateCaesarChallenge();
                return true;

            case "LOG ANALYSIS":
                (prompt, answer) = GenerateLogAnalysisChallenge();
                return true;

            case "BASE64":
                (prompt, answer) = GenerateBase64Challenge();
                return true;

            case "URL ANALYSIS":
                (prompt, answer) = GenerateUrlAnalysisChallenge();
                return true;

            default:
                prompt = string.Empty;
                answer = string.Empty;
                return false;
        }
    }

    private static (string Prompt, string Answer) GeneratePasswordChallenge()
    {
        string firstOption =
            $"password{RandomNumberGenerator.GetInt32(100000, 1_000_000)}";
        string secondOption =
            $"qwerty{RandomNumberGenerator.GetInt32(100000, 1_000_000)}";
        string generatedPassword = GenerateStrongPassword();

        string prompt =
            $"Choose the strongest password: {firstOption}, {secondOption}, or {generatedPassword}";

        return (prompt, generatedPassword);
    }

    private static string GenerateStrongPassword()
    {
        const string characters =
            "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz23456789!@#$%&*";

        char[] password = new char[12];

        for (int i = 0; i < password.Length; i++)
        {
            password[i] =
                characters[RandomNumberGenerator.GetInt32(characters.Length)];
        }

        return new string(password);
    }

    private static (string Prompt, string Answer) GenerateCaesarChallenge()
    {
        const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        char[] answer = new char[8];

        for (int i = 0; i < answer.Length; i++)
        {
            answer[i] = letters[RandomNumberGenerator.GetInt32(letters.Length)];
        }

        int shift = RandomNumberGenerator.GetInt32(1, 26);
        char[] encrypted = answer.ToArray();

        for (int i = 0; i < encrypted.Length; i++)
        {
            encrypted[i] =
                (char)('A' + (encrypted[i] - 'A' + shift) % 26);
        }

        string prompt =
            $"Decode this Caesar cipher by shifting backward {shift} letters: {new string(encrypted)}";

        return (prompt, new string(answer));
    }

    private static (string Prompt, string Answer) GenerateLogAnalysisChallenge()
    {
        HashSet<string> addresses = new();

        while (addresses.Count < 3)
        {
            addresses.Add(GeneratePrivateIp());
        }

        string[] ips = addresses.ToArray();
        string targetIp = ips[0];

        List<string> events = new()
        {
            $"08:14:03 FAILED login from {targetIp} user=admin",
            $"08:14:17 FAILED login from {targetIp} user=admin",
            $"08:14:39 FAILED login from {targetIp} user=admin",
            $"08:15:02 FAILED login from {targetIp} user=admin",
            $"08:15:11 FAILED login from {ips[1]} user=guest",
            $"08:15:26 SUCCESS login from {ips[2]} user=analyst"
        };

        for (int i = events.Count - 1; i > 0; i--)
        {
            int j = RandomNumberGenerator.GetInt32(i + 1);
            (events[i], events[j]) = (events[j], events[i]);
        }

        string prompt =
            "Which source IP had four failed login attempts? " +
            string.Join(" | ", events);

        return (prompt, targetIp);
    }

    private static string GeneratePrivateIp()
    {
        return $"10.{RandomNumberGenerator.GetInt32(0, 256)}." +
               $"{RandomNumberGenerator.GetInt32(0, 256)}." +
               $"{RandomNumberGenerator.GetInt32(1, 255)}";
    }

    private static (string Prompt, string Answer) GenerateBase64Challenge()
    {
        const string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        char[] answer = new char[8];

        for (int i = 0; i < answer.Length; i++)
        {
            answer[i] =
                characters[RandomNumberGenerator.GetInt32(characters.Length)];
        }

        string plainText = new string(answer);
        string encoded =
            Convert.ToBase64String(Encoding.UTF8.GetBytes(plainText));

        return ($"Decode this Base64 string: {encoded}", plainText);
    }

    private static (string Prompt, string Answer) GenerateUrlAnalysisChallenge()
    {
        string safeUrl = $"https://example.com/{GenerateRandomUrlPart(8)}";

        string[] urls =
        {
            safeUrl,
            $"https://example.com.{GenerateRandomUrlPart(6)}.test/{GenerateRandomUrlPart(8)}",
            $"https://example{RandomNumberGenerator.GetInt32(100, 1000)}.test/{GenerateRandomUrlPart(8)}"
        };

        for (int i = urls.Length - 1; i > 0; i--)
        {
            int j = RandomNumberGenerator.GetInt32(i + 1);
            (urls[i], urls[j]) = (urls[j], urls[i]);
        }

        string prompt =
            "Which URL has the exact hostname example.com? " +
            string.Join(" | ", urls);

        return (prompt, safeUrl);
    }

    private static string GenerateRandomUrlPart(int length)
    {
        const string characters = "abcdefghijklmnopqrstuvwxyz0123456789";
        char[] value = new char[length];

        for (int i = 0; i < value.Length; i++)
        {
            value[i] =
                characters[RandomNumberGenerator.GetInt32(characters.Length)];
        }

        return new string(value);
    }
}