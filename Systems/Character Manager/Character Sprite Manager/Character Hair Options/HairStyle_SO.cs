using UnityEngine;

[CreateAssetMenu(fileName = "Character Hair Style", menuName = "Custom Menus/Character Settings/Character Settings Scriptable Objects/Character Hair Style")]
public class HairStyle_SO : ScriptableObject
{
    [SerializeField] private HairSelection hairID;
    [SerializeField] private Sprite hairBack;
    [SerializeField] private Sprite hairFront_01;
    [SerializeField] private Sprite hairFront_02;
    [SerializeField] private Sprite hairMiddle;
    [SerializeField] private Sprite eyeBrow;
    [SerializeField] private Sprite hairIcon;

    public HairSelection HairID => hairID;
    public Sprite HairBack => hairBack;
    public Sprite HairFront_01 => hairFront_01;
    public Sprite HairFront_02 => hairFront_02;
    public Sprite HairMiddle => hairMiddle;
    public Sprite EyeBrow => eyeBrow;
    public Sprite HairIcon => hairIcon;
}
