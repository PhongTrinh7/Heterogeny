using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public abstract class Unit : MovingObject
{
    public enum UnitState{
        ACTIVE,
        BUSY,
        IDLE,
        READYUP
    }

    public UnitState state;
    public UnitState priorState;

    public UnitState State
    {
        get { return state; }
        set { state = value; }
    }

    //General info.
    public string unitName;
    public Portrait portrait;
    public Portrait tempPortrait;

    //Character UI
    public HealthBar healthBar;
    public EnergyBar energyBar;
    public GameObject turnIndicator;

    //Unit stats.
    public int spd;
    public int maxEnergy;
    protected int currentEnergy;
    public int CurrentEnergy
    {
        get { return currentEnergy; }
        set
        {
            //If value is changed, invoke the event.
            if (currentEnergy == value) return;
            currentEnergy = value;
            if (OnVariableChange != null)
                OnVariableChange.Invoke();
        }
    }
    public int energyRegen;

    //Unit Abilities.
    public List<Ability> abilitiesReference;
    public List<Ability> abilities;
    protected int loadedAbility;

    //Movement
    protected float inverseMoveTime;
    public Vector2 facingDirection;
    public int moveCost;

    //Status Afflictions
    public GameObject statusEffectContainer;
    public List<StatusEffect> statuses;
    public bool stunned;
    public bool immobilized;

    //Event for variable changes.
    UnityEvent OnVariableChange = new UnityEvent();

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        //UI setup.
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetCurrentHealth(currentHealth);
        energyBar.SetMaxEnergy(maxEnergy);

        OnVariableChange.AddListener(UpdateEnergy);
    }

    protected virtual void OnEnable()
    {
        currentEnergy = maxEnergy;
        UpdateEnergy();
        RefreshAbilities();
    }

    //Use cloned abilities as to not modify original Scriptable Object.
    protected void RefreshAbilities()
    {
        loadedAbility = -1;
        abilities.Clear();

        //Refresh ability cooldowns each battle.
        foreach (Ability a in abilitiesReference)
        {
            Ability ability = Object.Instantiate(a);
            ability.caster = this;
            abilities.Add(ability);
        }
    }

    //Move returns true if it is able to move and false if not. 
    //Move takes parameters for x direction, y direction and a RaycastHit2D to check collision.
    public IEnumerator Move(int xDir, int yDir)
    {
        if (stunned || immobilized)
        {
            Debug.Log("Can't move");
            yield break;
        }

        priorState = state;
        UnitStateChange(UnitState.BUSY);

        Vector2 newFacingDirection = new Vector2(xDir, yDir);
        ChangeFacingDirection(newFacingDirection);

        if (CurrentEnergy >= moveCost)
        {
            //Store start position to move from, based on objects current transform position.
            Vector2 start = transform.position;

            // Calculate end position based on the direction parameters passed in when calling Move.
            Vector2 end = start + facingDirection;

            RaycastHit2D[] hit;

            CastLineMaskDetect(end, end, blockingLayer, out hit);

            //Check if anything was hit
            if (hit.Length == 0)
            {
                CurrentEnergy -= moveCost;

                //Movement animation.
                anim.SetBool("Moving", true);
                yield return StartCoroutine(SmoothMovement(end, 1f));
            }
        }

        anim.SetBool("Moving", false);
        ReturnToPriorState();
    }

    public void ChangeFacingDirection(Vector2 direction)
    {
        if (stunned)
        {
            return;
        }

        //Sets facing direction.
        facingDirection = direction;
        anim.SetFloat("Horizontal", direction.x);
        anim.SetFloat("Vertical", direction.y);
    }

    public override void TakeDamage(int damage, Element e)
    {
        if (e == weakness) damage *= 2;
        currentHealth -= damage;
        healthBar.SetCurrentHealth(currentHealth);
        Instantiate(damageNumber, transform.position, Quaternion.identity).SetDamageVisual(damage, e, false);

        if (currentHealth <= 0)
        {
            Death();
        }
    }

    public bool ReadyAbility(int i)
    {
        priorState = state;
        UnitStateChange(UnitState.READYUP);

        if (CurrentEnergy >= abilities[i].cost)
        {
            if (abilities[i].Ready())
            {
                loadedAbility = i;
                return true;
            }
            else
            {
                Debug.Log(abilities[i] + " is on cooldown.");
                ReturnToPriorState();
                return false;
            }
        }
        else
        {
            Debug.Log("Not enough energy!");
            ReturnToPriorState();
            return false;
        }
    }

    public virtual void CastAbility()
    {
        UnitStateChange(UnitState.BUSY);
        CurrentEnergy -= abilities[loadedAbility].cost;
        abilities[loadedAbility].Cast();
    }

    public void TriggerAnimation(string animationName)
    {
        anim.SetTrigger(animationName);
    }

    public void CastAbilityEffect(int i)
    {
        abilities[loadedAbility].Effect(i);
    }

    public void PlaceHazardWave(EnvironmentalHazard hazard, List<List<Vector3>> waves, float waveDelay)
    {
        StartCoroutine(PlaceHazardWaveCoroutine(hazard, waves, waveDelay));
    }

    IEnumerator PlaceHazardWaveCoroutine(EnvironmentalHazard hazard, List<List<Vector3>> waves, float waveDelay)
    {
        foreach (List<Vector3> wave in waves)
        {
            foreach (Vector3 position in wave)
            {
                Instantiate(hazard, position, Quaternion.identity, ((Board)FindObjectOfType(typeof(Board))).transform);
            }
            yield return new WaitForSeconds(waveDelay);
        }
    }

    public virtual void UnitStateChange(UnitState newState)
    {
        state = newState;
    }

    public void ReturnToPriorState()
    {
        state = priorState;
        UIManager.Instance.ListenForBattleUI(state == UnitState.ACTIVE);
        Debug.Log("Unit state is now: " + state);
    }
    public void ApplyStatus(StatusEffect status)
    {
        status.OnApply(this);
    }

    public void ApplyEffects()
    {
        foreach (StatusEffect status in statuses.ToArray())
        {
            status.Effect();
            status.CheckTimer();
        }
    }

    public void HandleCooldowns()
    {
        foreach (Ability a in abilities)
        {
            a.Cooldown();
        }
    }

    public virtual void StartTurn()
    {
        turnIndicator.SetActive(true);
        CurrentEnergy = Mathf.Clamp(CurrentEnergy + energyRegen, 0, maxEnergy);
        HandleCooldowns();
        ApplyEffects();
    }

    public void EndTurn()
    {
        turnIndicator.SetActive(false);
        state = UnitState.IDLE;
        BattleManager.Instance.currentUnits.RemoveAt(0);
        BattleManager.Instance.currentUnits.Add(this);
        BattleManager.Instance.AdvanceTurn();
    }

    public void UpdateEnergy()
    {
        energyBar.SetCurrentEnergy(CurrentEnergy);
    }

    protected override void Death()
    {

        //Play animation or do whatever.
        
        //Inform the Battle Manager a Unit has died.
        BattleManager.Instance.UnitDeath(this);
    }
}