using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;

namespace OldShit.Items
{
	public class VirtualBlade : ModItem
	{
		public override void SetStaticDefaults() {
			Tooltip.SetDefault("As worthless as the real one."); // The (English) text shown below your weapon's name.

		}

		public override void SetDefaults() {
			Item.width = 90; // The item texture's width.
			Item.height = 90; // The item texture's height.

			Item.useStyle = ItemUseStyleID.Swing; // The useStyle of the Item.
			Item.useTime = 80; // The time span of using the weapon. Remember in terraria, 60 frames is a second.
			Item.useAnimation = 60; // The time span of the using animation of the weapon, suggest setting it the same as useTime.
			Item.autoReuse = true; // Whether the weapon can be used more than once automatically by holding the use button.

			Item.DamageType = DamageClass.Melee; // Whether your item is part of the melee class.
			Item.damage = 160; // The damage your item deals.
			Item.knockBack = 15f; // The force of knockback of the weapon. Maximum is 20
			Item.crit = 6; // The critical strike chance the weapon has. The player, by default, has a 4% critical strike chance.

			Item.value = Item.buyPrice(gold: 1); // The value of the weapon in copper coins.
			Item.rare = 9; // Give this item our custom rarity.
			Item.UseSound = SoundID.Item1; // The sound when the weapon is being used.
		}



		public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit) {

			target.AddBuff(BuffID.Venom, 90);
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.DD2SquireBetsySword, 1);
			recipe.AddIngredient(ItemID.Bladetongue, 3);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}