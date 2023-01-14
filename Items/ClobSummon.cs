using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using OldShit.NPCs;


namespace OldShit.Items
{
	// This is the item used to summon a boss, in this case the modded Minion Boss from Example Mod. For vanilla boss summons, see comments in SetStaticDefaults
	public class WeirdFluid: ModItem
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Abnormal Fluid");
			Tooltip.SetDefault("Awaken The Clobber Globber!");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
			ItemID.Sets.SortingPriorityBossSpawns[Type] = 12; // This helps sort inventory know that this is a boss summoning Item.

			// If this would be for a vanilla boss that has no summon item, you would have to include this line here:
			// NPCID.Sets.MPAllowedEnemies[NPCID.Plantera] = true;

			// Otherwise the UseItem code to spawn it will not work in multiplayer
		}

		public override void SetDefaults() {
			Item.width = 20;
			Item.height = 20;
			Item.maxStack = 20;
			Item.value = 100;
			Item.rare = ItemRarityID.Blue;
			Item.useAnimation = 30;
			Item.useTime = 30;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.consumable = true;
		}

		

		public override bool? UseItem(Player player) {
			if (player.whoAmI == Main.myPlayer) {
				// If the player using the item is the client
				// (explicitely excluded serverside here)
				SoundEngine.PlaySound(SoundID.ScaryScream, player.position);

				int type = ModContent.NPCType<Oldshit.NPCs.Bosses.Clobber.Clobberglobber>();

				if (Main.netMode != NetmodeID.MultiplayerClient) {
					// If the player is not in multiplayer, spawn directly
					NPC.SpawnOnPlayer(player.whoAmI, type);
				}
				else {
					// If the player is in multiplayer, request a spawn
					// This will only work if NPCID.Sets.MPAllowedEnemies[type] is true, which we set in MinionBossBody
					NetMessage.SendData(MessageID.SpawnBoss, number: player.whoAmI, number2: type);
				}
			}

			return true;
		}

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.LesserHealingPotion, 2)
                .AddIngredient(ItemID.Pumpkin, 5)
				.AddTile(TileID.DemonAltar)
				.Register();
		}
	}
}