using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class IType
{
    public enum ElementType { Heat, Breeze, Forest, Music, Space, Water, NoType };
    public enum BuffType {Strength,Speed,Shield,Heal,Relax,Split,Combo,Linger};
    public enum GemType { Heat, Breeze, Forest, Music, Space, Water };
    public enum MoveType { InstantAttack, InstantPassive, DelayedPassive, DelayedAttack };

    public enum Stat {Attack, Defense, Speed, Health, Energy};
    public enum QuestType {KillQuest, CollectQuest,TalkQuest};
    public enum EnemyType { Coconut,Mushroom};


}
