using UnityEngine;
using Unity.Mathematics;
using System.Collections.Generic;

public class MRNASequence
{
    private int leadBuffer = 5;
    private int trailBuffer = 6;

    private string fullSequence = "cucaaaagucuagagccaccguccagggagcagguagcugcugggcuccggggacacuuugcguucgggcugggagcgugcuuuccacgacggugacacgcuucccuggauuggcagccagacugccuuccgggucacugccauggaggagccgcagucagauccuagcgucgagcccccucugagucaggaaacauuuucagaccuauggaaacuacuuccugaaaacaacguucugucccccuugccgucccaagcaauggaugauuugaugcuguccccggacgauauugaacaaugguucacugaagacccagguccagaugaagcucccagaaugccagaggcugcuccccccguggccccugcaccagcagcuccuacaccggcggccccugcaccagcccccuccuggccccugucaucuucugucccuucccagaaaaccuaccagggcagcuacgguuuccgucugggcuucuugcauucugggacagccaagucugugacuugcacguacuccccugcccucaacaagauguuuugccaacuggccaagaccugcccugugcagcuguggguugauuccacacccccgcccggcacccgcguccgcgccauggccaucuacaagcagucacagcacaugacggagguugugaggcgcugcccccaccaugagcgcugcucagauagcgauggucuggccccuccucagcaucuuauccgaguggaaggaaauuugcguguggaguauuuggaugacagaaacacuuuucgacauagugugguggugcccuaugagccgccugagguuggcucugacuguaccaccauccacuacaacuacauguguaacaguuccugcaugggcggcaugaaccggaggcccauccucaccaucaucacacuggaagacuccagugguaaucuacugggacggaacagcuuugaggugcguguuugugccuguccugggagagaccggcgcacagaggaagagaaucuccgcaagaaaggggagccucaccacgagcugcccccagggagcacuaagcgagcacugcccaacaacaccagcuccucuccccagccaaagaagaaaccacuggauggagaauauuucacccuucagauccgugggcgugagcgcuucgagauguuccgagagcugaaugaggccuuggaacucaaggaugcccaggcugggaaggagccaggggggagcagggcucacuccagccaccugaaguccaaaaagggucagucuaccucccgccauaaaaaacucauguucaagacagaagggccugacucagacugacauucuccacuucuuguuccccacugacagccucccacccccaucucucccuccccugccauuuuggguuuugggucuuugaacccuugcuugcaauaggugugcgucagaagcacccaggacuuccauuugcuuugucccggggcuccacugaacaaguuggccugcacugguguuuuguuguggggaggaggauggggaguaggacauaccagcuuagauuuuaagguuuuuacugugagggauguuugggagauguaagaaauguucuugcaguuaaggguuaguuuacaaucagccacauucuagguaggggcccacuucaccguacuaaccagggaagcugucccucacuguugaauuuucucuaacuucaaggcccauaucugugaaaugcuggcauuugcaccuaccucacagagugcauugugaggguuaaugaaauaauguacaucuggccuugaaaccaccuuuuauuacauggggucuagaacuugacccccuugagggugcuuguucccucucccuguuggucgguggguugguaguuucuacaguugggcagcugguuagguagagggaguugucaagucucugcuggcccagccaaacccugucugacaaccucuuggugaaccuuaguaccuaaaaggaaaucucaccccaucccacacccuggaggauuucaucucuuguauaugaugaucuggauccaccaagacuuguuuuaugcucagggucaauuucuuuuuucuuuuuuuuuuuuuuuuuucuuuuucuuugagacugggucucgcuuuguugcccaggcuggaguggaguggcgugaucuuggcuuacugcagccuuugccuccccggcucgagcaguccugccucagccuccggaguagcugggaccacagguucaugccaccauggccagccaacuuuugcauguuuuguagagauggggucucacaguguugcccaggcuggucucaaacuccugggcucaggcgauccaccugucucagccucccagagugcugggauuacaauugugagccaccacguccagcuggaagggucaacaucuuuuacauucugcaagcacaucugcauuuucaccccacccuuccccuccuucucccuuuuuauaucccauuuuuauaucgaucucuuauuuuacaauaaaacuuugcugcca";
    private string leadingSequence = "cucaaaagucuagagccaccguccagggagcagguagcugcugggcuccggggacacuuugcguucgggcugggagcgugcuuuccacgacggugacacgcuucccuggauuggcagccagacugccuuccgggucacugcc";
    private string puzzleSequence = "auggaggagccgcagucagauccuagcgucgagcccccucugagucaggaaacauuuucagaccuauggaaacuacuuccugaaaacaacguucugucccccuugccgucccaagcaauggaugauuugaugcuguccccggacgauauugaacaaugguucacugaagacccagguccagaugaagcucccagaaugccagaggcugcuccccccguggccccugcaccagcagcuccuacaccggcggccccugcaccagcccccuccuggccccugucaucuucugucccuucccagaaaaccuaccagggcagcuacgguuuccgucugggcuucuugcauucugggacagccaagucugugacuugcacguacuccccugcccucaacaagauguuuugccaacuggccaagaccugcccugugcagcuguggguugauuccacacccccgcccggcacccgcguccgcgccauggccaucuacaagcagucacagcacaugacggagguugugaggcgcugcccccaccaugagcgcugcucagauagcgauggucuggccccuccucagcaucuuauccgaguggaaggaaauuugcguguggaguauuuggaugacagaaacacuuuucgacauagugugguggugcccuaugagccgccugagguuggcucugacuguaccaccauccacuacaacuacauguguaacaguuccugcaugggcggcaugaaccggaggcccauccucaccaucaucacacuggaagacuccagugguaaucuacugggacggaacagcuuugaggugcguguuugugccuguccugggagagaccggcgcacagaggaagagaaucuccgcaagaaaggggagccucaccacgagcugcccccagggagcacuaagcgagcacugcccaacaacaccagcuccucuccccagccaaagaagaaaccacuggauggagaauauuucacccuucagauccgugggcgugagcgcuucgagauguuccgagagcugaaugaggccuuggaacucaaggaugcccaggcugggaaggagccaggggggagcagggcucacuccagccaccugaaguccaaaaagggucagucuaccucccgccauaaaaaacucauguucaagacagaagggccugacucagacuga";
    private string trailingSequence = "cauucuccacuucuuguuccccacugacagccucccacccccaucucucccuccccugccauuuuggguuuugggucuuugaacccuugcuugcaauaggugugcgucagaagcacccaggacuuccauuugcuuugucccggggcuccacugaacaaguuggccugcacugguguuuuguuguggggaggaggauggggaguaggacauaccagcuuagauuuuaagguuuuuacugugagggauguuugggagauguaagaaauguucuugcaguuaaggguuaguuuacaaucagccacauucuagguaggggcccacuucaccguacuaaccagggaagcugucccucacuguugaauuuucucuaacuucaaggcccauaucugugaaaugcuggcauuugcaccuaccucacagagugcauugugaggguuaaugaaauaauguacaucuggccuugaaaccaccuuuuauuacauggggucuagaacuugacccccuugagggugcuuguucccucucccuguuggucgguggguugguaguuucuacaguugggcagcugguuagguagagggaguugucaagucucugcuggcccagccaaacccugucugacaaccucuuggugaaccuuaguaccuaaaaggaaaucucaccccaucccacacccuggaggauuucaucucuuguauaugaugaucuggauccaccaagacuuguuuuaugcucagggucaauuucuuuuuucuuuuuuuuuuuuuuuuuucuuuuucuuugagacugggucucgcuuuguugcccaggcuggaguggaguggcgugaucuuggcuuacugcagccuuugccuccccggcucgagcaguccugccucagccuccggaguagcugggaccacagguucaugccaccauggccagccaacuuuugcauguuuuguagagauggggucucacaguguugcccaggcuggucucaaacuccugggcucaggcgauccaccugucucagccucccagagugcugggauuacaauugugagccaccacguccagcuggaagggucaacaucuuuuacauucugcaagcacaucugcauuuucaccccacccuuccccuccuucucccuuuuuauaucccauuuuuauaucgaucucuuauuuuacaauaaaacuuugcugcca";

    private List<int3> sequenceSets;
    public List<int3> SequenceSets => sequenceSets;

    public int StartingIndex => leadingSequence.Length / 3;
    public int EndingIndex => StartingIndex + (puzzleSequence.Length / 3);

    public int LeadBuffer => leadBuffer;
    public int TrailBuffer => trailBuffer;

    public void ParseSequence()
    {
        sequenceSets = new List<int3>();
        int3 setToAdd = new int3();

        string trimmedString = 
            leadingSequence.Substring(leadingSequence.Length - leadBuffer * 3)
            + puzzleSequence
            + trailingSequence.Substring(0, trailBuffer * 3);
        for (int i = 0; i < trimmedString.Length; i += 3)
        {
            setToAdd.x = MRNAMap.CharToInt(trimmedString[i]);
            setToAdd.y = MRNAMap.CharToInt(trimmedString[i + 1]);
            setToAdd.z = MRNAMap.CharToInt(trimmedString[i + 2]);

            sequenceSets.Add(setToAdd);
        }
    }

    public void DebugLogSequence()
    {
        ParseSequence();
        foreach (int3 set in sequenceSets)
        {
            Debug.Log($"{set} == {MRNAMap.Int3ToString(set)}");
        }
    }

    public void DebugLogSequence(int start, int end)
    {
        ParseSequence();

        for (int i = start; i <= end; i++)
        {
            int3 set = sequenceSets[i];
            Debug.Log($"{set} == {MRNAMap.Int3ToString(set)}");
        }
    }

    public void DebugLogSequencePairs(int start, int end)
    {
        ParseSequence();

        for (int i = start; i <= end; i++)
        {
            int3 set = sequenceSets[i];
            Debug.Log($"{set} == {MRNAMap.GetInt3Pair(set)}");
        }
    }
}
