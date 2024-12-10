[System.Serializable]
public class MRNAPoolObject
{
    public MRNASet mRNASet;
    public bool inUse;

    public MRNAPoolObject(MRNASet mRNASet, bool inUse)
    {
        this.mRNASet = mRNASet;
        this.inUse = inUse;
    }
}
