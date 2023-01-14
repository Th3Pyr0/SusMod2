using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using OldShit.Systematic;

namespace OldShit.Items
{
	/// <summary>
	///     Star Wrath/Starfury style weapon. Spawn projectiles from sky that aim towards mouse.
	///     See Source code for Star Wrath projectile to see how it passes through tiles.
	///     For a detailed sword guide see <see cref="ExampleSword" />
	/// </summary>

	
	public class Yamato : ModItem
	{
		float gonemental;
		int elixirgolem;
		       
		public override void SetStaticDefaults() {
			Tooltip.SetDefault("Suffering defeat after defeat...\nThat man's body was breaking down.");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			Item.width = 62;
			Item.height = 66;
            Item.scale = 1.05f;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 15;
			Item.useAnimation = 15;
			Item.autoReuse = true;

			Item.DamageType = DamageClass.Melee;
			Item.damage = 100;
			Item.knockBack = 8;
			Item.crit = 40;

			Item.value = Item.buyPrice(platinum: 2);
			Item.rare = ItemRarityID.Purple;
			Item.UseSound = SoundID.Item1;

			Item.shoot = ModContent.ProjectileType<Oldshit.Items.Projectiles.Approaching>(); // ID of the projectiles the sword will shoot
			Item.shootSpeed = 16f; 
			
			// Speed of the projectiles the sword will shoot

			// If you want melee speed to only affect the swing speed of the weapon and not the shoot speed (not recommended)
			// Item.attackSpeedOnlyAffectsWeaponAnimation = true;
		}


		
		public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit){
			Item.damage += 5;
			if (Item.damage == 300){
				Item.damage = 100;
				//when used, slowly increases damage. TO DO: add timer to it, as this is based off of SSS in the real game.
				}
					//TO DO: add timer to it, as this is based off of SSS in the real game.
			}
			
		
		
		


		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Muramasa, 1);
			recipe.AddIngredient(ItemID.TerraBlade, 1);
            recipe.AddIngredient(ItemID.Terragrim, 1);
			recipe.AddIngredient(ItemID.BeetleHusk, 300);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}