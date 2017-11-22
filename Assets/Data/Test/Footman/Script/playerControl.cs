using UnityEngine;
using System.Collections;

public class playerControl : MonoBehaviour 
{
	public Animator anim;
	int attack01;
	int attack02;
	int attack03;
	int battleWalkBackward;
	int battleWalkForward;
	int battleWalkLeft;
	int battleWalkRight;
	int defend;
	int die;
	int getHit;
	int idle02;
	int jump;
	int walk;
	int taunt;
	int run;

    int reset;
    int current;

	void Awake () 
	{
		anim = GetComponent<Animator>();
		attack01 = Animator.StringToHash("attack_01");
		attack02 = Animator.StringToHash("attack_02");
		attack03 = Animator.StringToHash("attack_03");
		battleWalkBackward = Animator.StringToHash("walkBattleBackward");
		battleWalkForward = Animator.StringToHash("walkBattleForward");
		battleWalkLeft = Animator.StringToHash("walkBattleLeft");
		battleWalkRight = Animator.StringToHash("walkBattleRight");
		defend = Animator.StringToHash("defend");
		die = Animator.StringToHash("die");
		getHit = Animator.StringToHash("getHit");
		idle02 = Animator.StringToHash("idle_02");
		jump = Animator.StringToHash("jump");
		walk = Animator.StringToHash("walk");
		taunt = Animator.StringToHash("taunt");
		run = Animator.StringToHash("run");

        reset = Animator.StringToHash("idle_01");

        current = reset;
    }
	

	public void Attack01 ()
	{
        if (current != attack01)
        {
            Debug.Log("1");
            anim.SetTrigger(attack01);
            current = attack01;
        }
	}

	public void Attack02 ()
	{
		if (current != attack02)
        {
            Debug.Log("2");
            anim.SetTrigger(attack02);
            current = attack02;
        }
	}

	public void Attack03 ()
	{
        if (current != attack03)
        {
            Debug.Log("3");
            anim.SetTrigger(attack03);
            current = attack03;
        }
    }

	public void BattleWalkBackward ()
	{
        if (current != battleWalkBackward)
        {
            anim.SetTrigger(battleWalkBackward);
            current = battleWalkBackward;
        }
	}

	public void BattleWalkForward ()
	{
        if (current != battleWalkForward)
        {
            anim.SetTrigger(battleWalkForward);
            current = battleWalkForward;
        }
	}

	public void BattleWalkLeft ()
	{
        if (current != battleWalkLeft)
        {
            anim.SetTrigger(battleWalkLeft);
            current = battleWalkLeft;
        }
	}

	public void BattleWalkRight ()
	{
        if (current != battleWalkRight)
        {
            anim.SetTrigger(battleWalkRight);
            current = battleWalkRight;
        }
	}

	public void Defend ()
	{
        if (current != defend)
        {
            anim.SetTrigger(defend);
            current = defend;
        }
	}

	public void Die ()
	{
        if (current != die)
        {
            anim.SetTrigger(die);
            current = die;
        }
	}

	public void GetHit ()
	{
        if (current != getHit)
        {
            anim.SetTrigger(getHit);
            current = getHit;
        }
	}

	public void Idle02 ()
	{
        if (current != idle02)
        {
            anim.SetTrigger(idle02);
            current = idle02;
        }
	}

	public void Jump ()
	{
        if (current != jump)
        {
            anim.SetTrigger(jump);
            current = jump;
        }
	}

	public void Walk ()
	{
        if (current != walk)
        {
            anim.SetTrigger(walk);
            current = walk;
        }
	}

	public void Taunt ()
	{
        if (current != taunt)
        {
            anim.SetTrigger(taunt);
            current = taunt;
        }
	}

	public void Run ()
	{
        if (current != run)
        {
            anim.SetTrigger(run);
            current = run;
        }
	}

    public void Reset()
    {
        if (current != reset)
        {
            anim.SetTrigger(reset);
            current = reset;
        }
    }
}
