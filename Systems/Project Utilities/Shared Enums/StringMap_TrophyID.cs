public static class StringMap_TrophyID
{
    public static string ConvertToString(PuzzleKey puzzleKey)
    {
        switch (puzzleKey)
        {
            case PuzzleKey.Cytoplasm_MicrotubuleBridge_01:
                return "Tube1Solved";
            case PuzzleKey.Cytoplasm_MicrotubuleBridge_02:
                return "Tube2Solved";
            case PuzzleKey.Mitochondria_Gear_01:
                return "GearsComplete";
            case PuzzleKey.Mitochondria_Disc_01:
                return "DiscsComplete";
            case PuzzleKey.Mitochondria_Paddle_01:
                return "PaddlesComplete";
            case PuzzleKey.Mitochondria_Oxygen_01:
                return "OxyComplete";
            case PuzzleKey.Mitochondria_Synthase_01:
                return "CrusherComplete";
            case PuzzleKey.Motor_MotorProtein_01:
                return "MotorComplete";
        }

        return null;
    }
}
