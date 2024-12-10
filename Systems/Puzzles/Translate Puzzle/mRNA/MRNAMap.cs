using Unity.Mathematics;

public static class MRNAMap
{
    public static string IntToChar(int i)
    {
        switch (i)
        {
            case 0:
                return "a";
            case 1:
                return "c";
            case 2:
                return "g";
            case 3:
                return "u";
        }

        throw new System.Exception("Unhandled int input: " + i);
    }

    public static int CharToInt(char character)
    {
        switch (character)
        {
            case 'a':
                return 0;
            case 'c':
                return 1;
            case 'g':
                return 2;
            case 'u':
                return 3;
        }

        throw new System.Exception("Unhandled char input");
    }

    public static int GetIntPair(int i)
    {
        switch (i)
        {
            case 0:
                return 3;
            case 1:
                return 2;
            case 2:
                return 1;
            case 3:
                return 0;
        }

        throw new System.Exception("Unhandled int input");
    }
    public static char GetCharPair(char character)
    {
        switch (character)
        {
            case 'a':
                return 'u';
            case 'c':
                return 'g';
            case 'g':
                return 'c';
            case 'u':
                return 'a';
        }

        throw new System.Exception("Unhandled char input");
    }

    public static string GetInt3Pair(int3 i3)
    {
        i3.x = GetIntPair(i3.x);
        i3.y = GetIntPair(i3.y);
        i3.z = GetIntPair(i3.z);

        return Int3ToString(i3);
    }

    public static string Int3ToString(int3 i3)
    {
        return $"{IntToChar(i3.x)}{IntToChar(i3.y)}{IntToChar(i3.z)}";
    }

    public static TRNAType Int3ToTRNAType(int3 i3)
    {
        int returnVal = 0;
        returnVal += 100 * i3.x;
        returnVal += 10 * i3.y;
        returnVal += 1 * i3.z;
        return (TRNAType)returnVal;
    }
}
