﻿using System.Collections.Generic;
using System.Linq;
using SabberStoneCore.Enums;

namespace SabberStoneCore.Model
{
	/// <summary>
	/// Represents an entity in game which behaves as an actor. 
	/// The actions it can perform, as well as actions it undergoes,
	/// are defined by this interface.
	/// 
	/// The properties defined by this type are non complex; they have a
	/// very superficial meaning.
	/// </summary>
	public partial interface ICharacter : IPlayable
	{
		/// <summary>
		/// <see cref="Enums.Race"/>
		/// </summary>
		Race Race { get; set; }

		/// <summary>
		/// Indicates if this character can continue performing actions.
		/// </summary>
		bool IsDead { get; }

		/// <summary>
		/// Indicates if this character has the possibility to attack.
		/// </summary>
		bool CanAttack { get; }

		/// <summary>
		/// Contains a sequence of valid targets, also Characters, which
		/// can be attacked.
		/// </summary>
		IEnumerable<ICharacter> ValidAttackTargets { get; }

		/// <summary>
		/// Indicates if the provided target is attackable.
		/// 
		/// A quick solution would be to test if there are no taunt minions
		/// present and the target is not immune.
		/// </summary>
		/// <param name="target"></param>
		/// <returns></returns>
		bool IsValidAttackTarget(ICharacter target);

		/// <summary>
		/// This character takes damage from a certain other entity.
		/// The source is NOT specifically another character, since cards
		/// player from hand can also deal damage.
		/// 
		/// </summary>
		/// <param name="source"></param>
		/// <param name="damage"></param>
		/// <returns></returns>
		bool TakeDamage(IPlayable source, int damage);

		/// <summary>
		/// Reverse of <see cref="TakeDamage(IPlayable, int)"/>
		/// </summary>
		/// <param name="source"></param>
		/// <param name="heal"></param>
		void TakeHeal(IPlayable source, int heal);

		/// <summary>
		/// This character receives a heal for it's maximum health.
		/// </summary>
		/// <param name="source"></param>
		void TakeFullHeal(IPlayable source);

		/// <summary>
		/// This character receives a specified amount of armor.
		/// </summary>
		/// <param name="source"></param>
		/// <param name="armor"></param>
		void GainArmor(IPlayable source, int armor);

		/// <summary>
		/// The amount of damage this character can output.
		/// </summary>
		int AttackDamage { get; set; }

		/// <summary>
		/// The amount of damage taken during the evaluation of one phase.
		/// </summary>
		int Damage { get; set; }

		/// <summary>
		/// The amount of damage this character is about to take.
		/// </summary>
		int PreDamage { get; set; }

		/// <summary>
		/// The amount of health this character has remaing. This value is returned
		/// AFTER SUBTRACTING the pending damage for this entity.
		/// </summary>
		int Health { get; set; }

		/// <summary>
		/// The starting amount of health of this character.
		/// </summary>
		int BaseHealth { get; }

		/// <summary>
		/// Amount of attacks this character has executed during this turn.
		/// </summary>
		int NumAttacksThisTurn { get; set; }

		//bool ShouldExitCombat { get; set; }

		/// <summary>
		/// This character is currently attacking another character.
		/// </summary>
		bool IsAttacking { get; set; }
		/// <summary>
		/// This character is currently defending against another character.
		/// </summary>
		bool IsDefending { get; set; }

		/// <summary>
		/// This character has the intention of attacking another character.
		/// This property is important in the PRE-ATTACK phase, where random
		/// effects can come into play.
		/// </summary>
		int ProposedAttacker { get; set; }

		/// <summary>
		/// This character has the intention of defending against another character.
		/// This property is important in the PRE-ATTACK phase, where random
		/// effects can come into play.
		/// </summary>
		int ProposedDefender { get; set; }


#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

		bool CantBeTargetedBySpells { get; set; }
		bool CantBeTargetedByOpponents { get; set; }
		bool CantBeTargetedByHeroPowers { get; set; }
		bool IsExhausted { get; set; }
		bool IsFrozen { get; set; }
		bool IsSilenced { get; set; }
		bool IsImmune { get; set; }
		bool HasTaunt { get; set; }
		bool HasWindfury { get; set; }

#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
	}

	/// <summary>
	/// A character is ALSO an entity. The Tag based property system is also applicable to instances
	/// of this type.
	/// 
	/// <seealso cref="ICharacter"/>
	/// <seealso cref="Playable{T}"/>
	/// </summary>
	/// <typeparam name="T">Subclass of entity.</typeparam>
	public abstract partial class Character<T> : Playable<T>, ICharacter where T : Entity
	{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

		public bool CantAttack
		{
			get { return this[GameTag.CANT_ATTACK] == 1; }
			set { this[GameTag.CANT_ATTACK] = value ? 1 : 0; }
		}

		public bool CantAttackHeroes
		{
			get { return this[GameTag.CANNOT_ATTACK_HEROES] == 1; }
			set { this[GameTag.CANNOT_ATTACK_HEROES] = value ? 1 : 0; }
		}

		public bool CantBeTargetedBySpells
		{
			get { return this[GameTag.CANT_BE_TARGETED_BY_SPELLS] == 1; }
			set { this[GameTag.CANT_BE_TARGETED_BY_SPELLS] = value ? 1 : 0; }
		}

		public bool CantBeTargetedByHeroPowers
		{
			get { return this[GameTag.CANT_BE_TARGETED_BY_HERO_POWERS] == 1; }
			set { this[GameTag.CANT_BE_TARGETED_BY_HERO_POWERS] = value ? 1 : 0; }
		}

		public int Armor
		{
			get { return this[GameTag.ARMOR]; }
			set { this[GameTag.ARMOR] = value; }
		}

		public int LastAffectedBy
		{
			get { return this[GameTag.LAST_AFFECTED_BY]; }
			set { this[GameTag.LAST_AFFECTED_BY] = value; }
		}

		public int AttackDamage
		{
			get { return this[GameTag.ATK]; }
			set { this[GameTag.ATK] = value; }
		}

		public bool CantBeTargetedByOpponents
		{
			get { return this[GameTag.CANT_BE_TARGETED_BY_OPPONENTS] == 1; }
			set { this[GameTag.CANT_BE_TARGETED_BY_OPPONENTS] = value ? 1 : 0; }
		}

		public int Damage
		{
			get { return this[GameTag.DAMAGE]; }
			set
			{
				if (this[GameTag.HEALTH] <= value)
				{
					ToBeDestroyed = true;
				}
				// don't allow negative values
				this[GameTag.DAMAGE] = value < 0 ? 0 : value;
			}
		}

		public int Health
		{
			get { return this[GameTag.HEALTH] - this[GameTag.DAMAGE]; }
			set
			{
				if (value == 0)
				{
					ToBeDestroyed = true;
				}

				this[GameTag.HEALTH] = value;
				this[GameTag.DAMAGE] = 0;
			}
		}

		public bool IsAttacking
		{
			get { return this[GameTag.ATTACKING] == 1; }
			set { this[GameTag.ATTACKING] = value ? 1 : 0; }
		}

		public bool IsDefending
		{
			get { return this[GameTag.DEFENDING] == 1; }
			set { this[GameTag.DEFENDING] = value ? 1 : 0; }
		}

		public int ProposedAttacker
		{
			get { return this[GameTag.PROPOSED_ATTACKER]; }
			set { this[GameTag.PROPOSED_ATTACKER] = value; }
		}

		public int ProposedDefender
		{
			get { return this[GameTag.PROPOSED_DEFENDER]; }
			set { this[GameTag.PROPOSED_DEFENDER] = value; }
		}

		public bool IsImmune
		{
			get { return this[GameTag.IMMUNE] == 1; }
			set { this[GameTag.IMMUNE] = value ? 1 : 0; }
		}

		public bool IsFrozen
		{
			get { return this[GameTag.FROZEN] == 1; }
			set { this[GameTag.FROZEN] = value ? 1 : 0; }
		}

		public bool IsSilenced
		{
			get { return this[GameTag.SILENCED] == 1; }
			set { this[GameTag.SILENCED] = value ? 1 : 0; }
		}

		public bool HasTaunt
		{
			get { return this[GameTag.TAUNT] == 1; }
			set { this[GameTag.TAUNT] = value ? 1 : 0; }
		}

		public virtual bool HasWindfury
		{
			get { return this[GameTag.WINDFURY] == 1; }
			set { this[GameTag.WINDFURY] = value ? 1 : 0; }
		}

		public int NumAttacksThisTurn
		{
			get { return this[GameTag.NUM_ATTACKS_THIS_TURN]; }
			set { this[GameTag.NUM_ATTACKS_THIS_TURN] = value; }
		}

		public int PreDamage
		{
			get { return this[GameTag.PREDAMAGE]; }
			set { this[GameTag.PREDAMAGE] = value; }
		}

		public Race Race
		{
			get { return (Race)this[GameTag.CARDRACE]; }
			set { this[GameTag.CARDRACE] = (int)value; }
		}

		public bool ShouldExitCombat
		{
			get { return this[GameTag.SHOULDEXITCOMBAT] == 1; }
			set { this[GameTag.SHOULDEXITCOMBAT] = value ? 1 : 0; }
		}

		public int BaseHealth
		{
			get { return this[GameTag.HEALTH]; }
			set { this[GameTag.HEALTH] = value; }
		}

		public bool IsDead => Health <= 0 || ToBeDestroyed;

		public virtual bool CanAttack => !CantAttack && !IsExhausted && !IsFrozen && ValidAttackTargets.Any();

#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

		/// <summary>
		/// Build a new character from the provided data.
		/// </summary>
		/// <param name="controller">Owner of the character; not specifically limited to players.</param>
		/// <param name="card">The card which this character embodies.</param>
		/// <param name="tags">Properties of this entity.</param>
		protected Character(Controller controller, Card card, Dictionary<GameTag, int> tags)
			: base(controller, card, tags)
		{
		}		

		/// <summary>
		/// Indicates if the provided character can be attacked by this character.
		/// </summary>
		/// <param name="target"></param>
		/// <returns></returns>
		public virtual bool IsValidAttackTarget(ICharacter target)
		{
			// got target but isn't contained in valid targets
			if (!ValidAttackTargets.Contains(target))
			{
				Game.Log(LogLevel.WARNING, BlockType.ACTION, "Character", $"{this} has an invalid target {target}.");
				return false;
			}

			var hero = target as Hero;
			if (CantAttackHeroes && (hero != null))
			{
				Game.Log(LogLevel.WARNING, BlockType.ACTION, "Character", $"Can't attack Heroes!");
				return false;
			}

			return true;
		}

		/// <summary>
		/// Returns a sequence of characters which are attackable.
		/// </summary>
		public IEnumerable<ICharacter> ValidAttackTargets
		{
			get
			{
				var allTargets = Controller.Opponent.Board.Where(x => !x.HasStealth && !x.IsImmune).ToList<ICharacter>();
				var allTargetsTaunt = allTargets.Where(x => x.HasTaunt).ToList();
				if (!CantAttackHeroes)
				{
					allTargets.Add(Controller.Opponent.Hero);
				}
				return allTargetsTaunt.Any() ? allTargetsTaunt : allTargets;
			}
		}

		/// <summary>
		/// Inflict damage onto this character.
		/// The actual amount still needs to be determined by the current
		/// state of the game.
		/// </summary>
		/// <param name="source"></param>
		/// <param name="damage"></param>
		/// <returns></returns>
		public bool TakeDamage(IPlayable source, int damage)
		{
			var hero = this as Hero;
			var minion = this as Minion;

			bool fatigue = hero != null && this == source;

			if (fatigue)
			{
				hero.Fatigue = damage;
			}

			if (minion != null && minion.HasDivineShield)
			{
				Game.Log(LogLevel.INFO, BlockType.ACTION, "Character", $"{this} divine shield absorbed incoming damage.");
				minion.HasDivineShield = false;
				return false;
			}

			if (minion != null && minion.IsImmune || hero != null && hero.IsImmune)
			{
				Game.Log(LogLevel.INFO, BlockType.ACTION, "Character", $"{this} is immune.");
				return false;
			}

			// remove armor first from hero ....
			if (hero != null && hero.Armor > 0)
			{
				if (hero.Armor < damage)
				{
					damage = damage - hero.Armor;
					hero.Armor = 0;
				}
				else
				{
					hero.Armor = hero.Armor - damage;
					damage = 0;
				}
			}

			// added pre damage to be able to interact
			PreDamage = damage;

			// final damage is beeing accumulated
			Damage += PreDamage;

			Game.Log(LogLevel.INFO, BlockType.ACTION, "Character", $"{this} took damage for {PreDamage}({damage}). {(fatigue ? "(fatigue)" : "")}");

			// check if there was damage done
			bool tookDamage = PreDamage > 0;

			// reset predamage
			PreDamage = 0;

			return tookDamage;
		}

		/// <summary>
		/// Heal up all taken damage.
		/// </summary>
		/// <param name="source"></param>
		public void TakeFullHeal(IPlayable source)
		{
			TakeHeal(source, Damage);
		}

		/// <summary>
		/// Heal a specified amount of health.
		/// 
		/// TODO The actual amount of healed damage must be determined from the state of the game?
		/// </summary>
		/// <param name="source"></param>
		/// <param name="heal"></param>
		public void TakeHeal(IPlayable source, int heal)
		{
			// we don't heal undamaged entities
			if (Damage == 0)
			{
				return;
			}

			int amount = Damage > heal ? heal : Damage;
			Game.Log(LogLevel.INFO, BlockType.ACTION, "Character", $"{this} took healing for {amount}.");
			Damage -= amount;
		}

		/// <summary>
		/// Gain the specified amount of armor.
		/// </summary>
		/// <param name="source"></param>
		/// <param name="armor"></param>
		public void GainArmor(IPlayable source, int armor)
		{
			Game.Log(LogLevel.INFO, BlockType.ACTION, "Character", $"{this} gaining armor for {armor}.");
			Armor += armor;
		}
	}
}