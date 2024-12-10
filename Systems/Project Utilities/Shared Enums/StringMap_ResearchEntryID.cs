public static class StringMap_ResearchEntryID
{
    private const string prefixString = "ResearchJournal_";

    public static string ConvertToString(ResearchEntryID researchEntryID)
    {
        string returnString = null;

        switch (researchEntryID)
        {
            case ResearchEntryID.None:
                break;
            case ResearchEntryID.ADP_ATP:
                returnString = "ADP-ATP";
                break;
            case ResearchEntryID.Centrosome:
                returnString = "Centrosome";
                break;
            case ResearchEntryID.CytoplasmicProteins:
                returnString = "CytoplasmicProteins";
                break;
            case ResearchEntryID.Dynein:
                returnString = "Dynein";
                break;
            case ResearchEntryID.ElectronTransportChain:
                returnString = "ElectronTransportChain";
                break;
            case ResearchEntryID.Electrons:
                returnString = "Electrons";
                break;
            case ResearchEntryID.ERProteins:
                returnString = "ERProteins";
                break;
            case ResearchEntryID.GolgiApparatus:
                returnString = "GolgiApparatus";
                break;
            case ResearchEntryID.Kinesin:
                returnString = "Kinesin";
                break;
            case ResearchEntryID.Membrane:
                returnString = "Membrane";
                break;
            case ResearchEntryID.Microtubule:
                returnString = "Microtubule";
                break;
            case ResearchEntryID.Mitochondria:
                returnString = "Mitochondria";
                break;
            case ResearchEntryID.MitochondrialDNA:
                returnString = "MitochondrialDNA";
                break;
            case ResearchEntryID.MitochondrialProteins:
                returnString = "MitochondrialProteins";
                break;
            case ResearchEntryID.Nucleus:
                returnString = "Nucleus";
                break;
            case ResearchEntryID.Protons:
                returnString = "Protons";
                break;
            case ResearchEntryID.RoughER:
                returnString = "RoughER";
                break;
            case ResearchEntryID.SmoothER:
                returnString = "SmoothER";
                break;
            case ResearchEntryID.Vesicle:
                returnString = "Vesicle";
                break;
            case ResearchEntryID.Ribosome:
                returnString = "Ribosome";
                break;
        }

        return prefixString + returnString;
    }
}
