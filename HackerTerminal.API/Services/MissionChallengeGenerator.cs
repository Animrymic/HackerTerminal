using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace HackerTerminal.API.Services;

public static class MissionChallengeGenerator
{
    public static bool TryGenerate(
        string? category,
        string? difficulty,
        out string prompt,
        out string answer)
    {
        string level = difficulty?.Trim().ToUpperInvariant() ?? "MEDIUM";

        switch (category?.Trim().ToUpperInvariant())
        {
            case "PASSWORD":
                (prompt, answer) = GeneratePasswordChallenge(level);
                return true;

            case "CIPHER":
                (prompt, answer) = GenerateCaesarChallenge(level);
                return true;

            case "LOG ANALYSIS":
                (prompt, answer) = GenerateLogAnalysisChallenge(level);
                return true;

            case "BASE64":
                (prompt, answer) = GenerateBase64Challenge(level);
                return true;

            case "URL ANALYSIS":
                (prompt, answer) = GenerateUrlAnalysisChallenge(level);
                return true;

            default:
                prompt = string.Empty;
                answer = string.Empty;
                return false;
        }
    }

    private static int DifficultyValue(
        string difficulty,
        int easy,
        int medium,
        int hard)
    {
        return difficulty switch
        {
            "EASY" => easy,
            "HARD" => hard,
            _ => medium
        };
    }

    private static (string Prompt, string Answer) GeneratePasswordChallenge(
     string difficulty)
    {
        string strongPasswordOne = GenerateStrongPassword(
            DifficultyValue(difficulty, 12, 14, 18));
        string strongPasswordTwo = GenerateStrongPassword(
            DifficultyValue(difficulty, 12, 14, 18));

        string weakPasswordOne =
            $"password{RandomNumberGenerator.GetInt32(100, 10_000)}";
        string weakPasswordTwo =
            $"qwerty{RandomNumberGenerator.GetInt32(100, 10_000)}";

        if (RandomNumberGenerator.GetInt32(2) == 0)
        {
            string prompt =
                $"Choose the strongest password: {weakPasswordOne}, " +
                $"{strongPasswordOne}, or {weakPasswordTwo}";

            return (prompt, strongPasswordOne);
        }

        string weakAnswer = RandomNumberGenerator.GetInt32(2) == 0
            ? weakPasswordOne
            : weakPasswordTwo;

        string weakPrompt =
            $"Which password is weak and should be rejected: " +
            $"{strongPasswordOne}, {weakAnswer}, or {strongPasswordTwo}?";

        return (weakPrompt, weakAnswer);
    }
    private static string GenerateStrongPassword(int length)
    {
        const string characters =
            "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz23456789!@#$%&*";

        char[] password = new char[length];

        for (int i = 0; i < password.Length; i++)
        {
            password[i] =
                characters[RandomNumberGenerator.GetInt32(characters.Length)];
        }

        return new string(password);
    }

    private static (string Prompt, string Answer) GenerateCaesarChallenge(
        string difficulty)
    {
        const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        int length = DifficultyValue(difficulty, 6, 8, 12);
        char[] answer = new char[length];

        for (int i = 0; i < answer.Length; i++)
        {
            answer[i] = letters[RandomNumberGenerator.GetInt32(letters.Length)];
        }

        int shift = RandomNumberGenerator.GetInt32(1, 26);
        bool encodedForward = RandomNumberGenerator.GetInt32(2) == 0;
        char[] encrypted = answer.ToArray();

        for (int i = 0; i < encrypted.Length; i++)
        {
            int offset = encrypted[i] - 'A';
            int encodedOffset = encodedForward
                ? (offset + shift) % 26
                : (offset - shift + 26) % 26;

            encrypted[i] = (char)('A' + encodedOffset);
        }

        string direction = encodedForward ? "backward" : "forward";
        string prompt =
            $"Decode this Caesar cipher by shifting {direction} {shift} " +
            $"letters: {new string(encrypted)}";

        return (prompt, new string(answer));
    }

    private static (string Prompt, string Answer) GenerateLogAnalysisChallenge(
        string difficulty)
    {
        int repeatedFailures = DifficultyValue(difficulty, 3, 4, 6);
        int addressCount = DifficultyValue(difficulty, 3, 4, 6);

        HashSet<string> addresses = new();

        while (addresses.Count < addressCount)
        {
            addresses.Add(GeneratePrivateIp());
        }

        string[] ips = addresses.ToArray();
        string targetIp = ips[0];
        string[] timestamps =
        {
            "08:14:03",
            "08:14:17",
            "08:14:39",
            "08:15:02",
            "08:15:11",
            "08:15:26"
        };

        List<string> events = new();

        for (int i = 0; i < repeatedFailures; i++)
        {
            events.Add(
                $"{timestamps[i]} FAILED login from {targetIp} user=admin");
        }

        for (int i = 1; i < ips.Length; i++)
        {
            events.Add(
                $"08:16:{i * 7:00} FAILED login from {ips[i]} user=guest");
            events.Add(
                $"08:17:{i * 6:00} SUCCESS login from {ips[i]} user=analyst");
        }

        for (int i = events.Count - 1; i > 0; i--)
        {
            int j = RandomNumberGenerator.GetInt32(i + 1);
            (events[i], events[j]) = (events[j], events[i]);
        }

        string prompt =
            $"Which source IP had {repeatedFailures} failed login attempts? " +
            string.Join(" | ", events);

        return (prompt, targetIp);
    }

    private static string GeneratePrivateIp()
    {
        return $"10.{RandomNumberGenerator.GetInt32(0, 256)}." +
               $"{RandomNumberGenerator.GetInt32(0, 256)}." +
               $"{RandomNumberGenerator.GetInt32(1, 255)}";
    }

    private static (string Prompt, string Answer) GenerateBase64Challenge(
        string difficulty)
    {
        const string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        int length = DifficultyValue(difficulty, 6, 8, 12);
        char[] plainTextCharacters = new char[length];

        for (int i = 0; i < plainTextCharacters.Length; i++)
        {
            plainTextCharacters[i] =
                characters[RandomNumberGenerator.GetInt32(characters.Length)];
        }

        string plainText = new string(plainTextCharacters);
        string encoded =
            Convert.ToBase64String(Encoding.UTF8.GetBytes(plainText));

        if (RandomNumberGenerator.GetInt32(2) == 0)
        {
            return ($"Decode this Base64 string: {encoded}", plainText);
        }

        return ($"Encode this text as Base64: {plainText}", encoded);
    }

    private static (string Prompt, string Answer) GenerateUrlAnalysisChallenge(
        string difficulty)
    {
        string[] knownHosts =
        {
            "example.com",
            "example.org",
            "example.net",
            "example.edu"
        };

        string targetHost =
            knownHosts[RandomNumberGenerator.GetInt32(knownHosts.Length)];

        string correctUrl =
            $"https://{targetHost}/{GenerateRandomUrlPart(8)}";

        int optionCount = DifficultyValue(difficulty, 3, 4, 5);
        List<string> urls = new() { correctUrl };

        for (int i = 1; i < optionCount; i++)
        {
            string randomPart = GenerateRandomUrlPart(6);
            string path = GenerateRandomUrlPart(8);

            string decoy = i switch
            {
                1 => $"https://{targetHost}.{randomPart}.test/{path}",
                2 => $"https://login.{targetHost}/{path}",
                3 => $"https://{targetHost}@{randomPart}.test/{path}",
                _ => $"https://{targetHost.Replace('.', '-')}.test/{path}"
            };

            urls.Add(decoy);
        }

        for (int i = urls.Count - 1; i > 0; i--)
        {
            int j = RandomNumberGenerator.GetInt32(i + 1);
            (urls[i], urls[j]) = (urls[j], urls[i]);
        }

        string prompt =
            $"Which URL has the exact hostname {targetHost}? " +
            string.Join(" | ", urls);

        return (prompt, correctUrl);
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