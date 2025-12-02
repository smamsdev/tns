using UnityEngine;

public class PlayerPermanentStats : PartyMemberSO
{
    public int MaxPotential
    {
        get { return maxPotential; }
        set
        {
            maxPotential = Mathf.Clamp(value, 1, 999);
        }
    }

    public int CurrentPotential
    {
        get { return currentPotential; }
        set
        {
            currentPotential = Mathf.Clamp(value, 0, 999);
        }
    }

    public int Smams
    {
        get { return smams; }
        set
        {
            smams = Mathf.Clamp(value, 0, 1000000);
        }
    }

    public int FocusBase
    {
        get { return focusBase; }
        set
        {
            focusBase = Mathf.Clamp(value, 1, 999);
        }
    }

    [SerializeField] private int maxPotential;
    [SerializeField] private int currentPotential;
    [SerializeField] private int focusBase;
    [SerializeField] private int smams;

}
