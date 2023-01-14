using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using Oldshit.Items;
using Oldshit.NPCs;
using OldShit;
using Terraria.Localization;
using OldShit.Systematic;
using Terraria.GameContent;
using Terraria.GameContent.ItemDropRules;
using Oldshit.Items.Projectiles;
using Terraria.Audio;
using System.Collections.Generic;
using System.IO;
using Terraria.Chat;

namespace Oldshit.NPCs.Bosses.Clobber
{
    [AutoloadBossHead]
    public class Clobberglobber : ModNPC
    {
        public Vector2 FirstStageDestination {
			get => new Vector2(NPC.ai[1], NPC.ai[2]);
			set {
				NPC.ai[1] = value.X;
				NPC.ai[2] = value.Y;
			}
		}
        public static int MinionType() {
			return NPCID.SkeletonArcher;
		}

        public static int MinionCount() {
			int count = 2;
            return count;
        }
        private float timer;
        float ProjectileTimer;
        float halfhpTimer;
        float maxp2timer;
        float MaxTimer;
        float expertAdd;
        private bool spawned = false;
        private bool spawned2 = false;
        private bool spawned3 = false;
        private bool spawned4 = false;

        public bool SecondStage {
			get => NPC.ai[0] == 1f;
			set => NPC.ai[0] = value ? 1f : 0f;
		}

        public ref float SecondStageTimer_ShootNuke => ref NPC.localAI[3];
        
        


        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Clobber Globber");
            Main.npcFrameCount[NPC.type] = 4;
        }

        public override void SetDefaults()
        {
            NPC.width = 64;
            NPC.height = 104;
            NPC.damage = 80;
            NPC.defense = 12;
            NPC.lifeMax = 6000;
            NPC.boss = true;
            NPC.DeathSound = SoundID.NPCDeath34;
            NPC.value = 60f;
            NPC.knockBackResist = 0f;
            AIType = NPCID.GraniteGolem;
            NPC.aiStyle = 91;
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            NPC.lavaImmune = true;

            AnimationType = -1;

            
            MaxTimer = 0;
            maxp2timer = 0; // this is the starting value which is important to assign to zero for the projectile code later on

            if (NPC.life<= 0) {
				Music = MusicID.Boss4;
			}
        }



        private void Talk(string message) {
            if (Main.netMode != NetmodeID.Server) {
                string text = Language.GetTextValue("Oldshit/NPCs/Bosses/Clobber", Lang.GetNPCNameValue(NPC.type), message);
                Main.NewText(text, 150, 250, 150);
            }
            else {
                NetworkText text = NetworkText.FromKey("Oldshit/NPCs/Bosses/Clobber", Lang.GetNPCNameValue(NPC.type), message);
                ChatHelper.BroadcastChatMessage(text, new Color(150, 250, 150));
            }
            
        }

        public override void AI()

        
        {
            maxp2timer = 240;
            
            if (Main.player[NPC.target].ZoneForest)
            {
                NPC.dontTakeDamage = false;
            }
            else
            {
                NPC.dontTakeDamage = true;
                if (NPC.life >= NPC.lifeMax * 0.5f || Main.expertMode && NPC.life >= NPC.lifeMax * (0.6f)) // here to make sure that the dashes which happen while outside of the biome don't interfere with the other dashes in the code
                {
                    timer++;
                    if (timer > 180) // after 120 ticks or 2 seconds
                    {
                        timer = 0; // reset the timer
                        NPC.velocity.X = Math.Sign(NPC.Center.X - Main.player[NPC.target].Center.X) * 26f; // make the NPC X speed go to the player
                                                                                                           // or
                        NPC.velocity = NPC.DirectionTo(Main.player[NPC.target].Center) * 13f; // point to the player's center (X and Y)
                    }
                }
            }

            Player player = Main.player[NPC.target];
            if (!player.active || player.dead)
            {
                NPC.TargetClosest();
                player = Main.player[NPC.target];
                if (!player.active || player.dead)
                {
                    NPC.active = false;
                    return;
                }
            }

            ProjectileTimer += 1f; // this starts the timer counting up by a value of 1 (ProjectileTimer += 1f; would do the same thing)

            
            { if (NPC.life <= 4000) {
				    DoSecondStage(player);
             }

            CheckSecondStage();

            if (ProjectileTimer >= 35) // this checks that the projectile's timer is over the maximum time you set
            {
                NPC.TargetClosest();
                if (NPC.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    var source = NPC.GetSource_FromAI();
                    Vector2 position = NPC.Center;
                    Vector2 targetPosition = Main.player[NPC.target].Center;
                    Vector2 direction = targetPosition - position;
                    direction.Normalize();
                    float speed = 5f;
                    int type = ProjectileID.PinkLaser;
                    int damage = 30; //If the projectile is hostile, the damage passed into NewProjectile will be applied doubled, and quadrupled if expert mode, so keep that in mind when balancing projectiles if you scale it off NPC.damage (which also increases for expert/master)
                    Projectile.NewProjectile(source, position, direction * speed, type, damage, 0f, Main.myPlayer);
                }
                 // this resets the timer to zero
            }  


            
            if (ProjectileTimer >= 49)
             {
                 NPC.TargetClosest();
                if (NPC.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    var source = NPC.GetSource_FromAI();
                    Vector2 position = NPC.Center;
                    Vector2 targetPosition = Main.player[NPC.target].Center;
                    Vector2 direction = targetPosition - position;
                    direction.Normalize();
                    float speed = 9f;
                    int type = ModContent.ProjectileType<OldShit.Items.Projectiles.ClobbedBalls>();
                    int damage = 50; //If the projectile is hostile, the damage passed into NewProjectile will be applied doubled, and quadrupled if expert mode, so keep that in mind when balancing projectiles if you scale it off NPC.damage (which also increases for expert/master)
                    Projectile.NewProjectile(source, position, direction * speed, type, damage, 0f, Main.myPlayer);
                  // sets this to zero to activate the code that generates a number
                    // this resets the timer to zero
                
                 }
             }
             
             if (ProjectileTimer >= MaxTimer)
             {
                /* NPC.TargetClosest();
                if (NPC.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    var source = NPC.GetSource_FromAI();
                    Vector2 position = NPC.Center;
                    Vector2 targetPosition = Main.player[NPC.target].Center;
                    Vector2 direction = targetPosition - position;
                    direction.Normalize();
                    float speed = 9f;
                    int type = ModContent.ProjectileType<OldShit.Items.Projectiles.ClobbedHead>();
                    int damage = 120; //If the projectile is hostile, the damage passed into NewProjectile will be applied doubled, and quadrupled if expert mode, so keep that in mind when balancing projectiles if you scale it off NPC.damage (which also increases for expert/master)
                    Projectile.NewProjectile(source, position, direction * speed, type, damage, 0f, Main.myPlayer);
                }
                */
                MaxTimer = 50; // sets this to zero to activate the code that generates a number
                ProjectileTimer = 0;
                
                //}
             }
            
            if (Main.expertMode)
                expertAdd = 0.5f;
                
			}
                

           
          }
        
        		private void CheckSecondStage() {
			if (SecondStage) {
				
				return;
			    }
             }

             private void DoSecondStage(Player player) {
                halfhpTimer =+ 1;
                maxp2timer = 240;

                if (halfhpTimer == maxp2timer){
                    DoSecondStage_ShootNuke(player);
                }
                 
                
                


			
			

			NPC.damage = NPC.damage*2;

			NPC.alpha = 0;

			
		}

        private void DoSecondStage_ShootNuke(Player player) {
			// At 100% health, spawn every 90 ticks
			// Drops down until 33% health to spawn every 30 ticks
			float timerMax = 90;

			SecondStageTimer_ShootNuke++;
			if (SecondStageTimer_ShootNuke > timerMax) {
				SecondStageTimer_ShootNuke = 0;
			}

			if (NPC.HasValidTarget && SecondStageTimer_ShootNuke == 0 && Main.netMode != NetmodeID.MultiplayerClient) {
				// Spawn projectile randomly below player, based on horizontal velocity to make kiting harder, starting velocity 1f upwards
				// (The projectiles accelerate from their initial velocity)

				float kitingOffsetX = Utils.Clamp(player.velocity.X * 16, -100, 100);
				Vector2 position = player.Bottom + new Vector2(kitingOffsetX + Main.rand.Next(-100, 100), Main.rand.Next(50, 100));

				int type = ModContent.ProjectileType<OldShit.Items.Projectiles.clobberbomber>();
				int damage = NPC.damage;
				var entitySource = NPC.GetSource_FromAI();

				Projectile.NewProjectile(entitySource, position, -Vector2.UnitY, type, damage, 0f, Main.myPlayer);
                }
			}
        public override void OnKill() {
			// This sets downedMinionBoss to true, and if it was false before, it initiates a lantern night
            NPC.TargetClosest();
                if (NPC.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    var source = NPC.GetSource_FromAI();
                    Vector2 position = NPC.Center;
                    Vector2 targetPosition = Main.player[NPC.target].Center;
                    Vector2 direction = targetPosition - position;
                    direction.Normalize();
                    float speed = 9f;
                    int type = ProjectileID.InfernoHostileBolt;
                    int damage = 120; //If the projectile is hostile, the damage passed into NewProjectile will be applied doubled, and quadrupled if expert mode, so keep that in mind when balancing projectiles if you scale it off NPC.damage (which also increases for expert/master)
                    Projectile.NewProjectile(source, position, direction * speed, type, damage, 0f, Main.myPlayer);
                }
            NPC.SetEventFlagCleared(ref DownedBossSystem.downedSolaris, -1);
        

			// Since this hook is only ran in singleplayer and serverside, we would have to sync it manually.
			// Thankfully, vanilla sends the MessageID.WorldData packet if a BOSS was killed automatically, shortly after this hook is ran

			// If your NPC is not a boss and you need to sync the world (which includes ModSystem, check DownedBossSystem), use this code:
			/*
			if (Main.netMode == NetmodeID.Server) {
				NetMessage.SendData(MessageID.WorldData);
			}
			*/
		}
        		public override void ModifyNPCLoot(NPCLoot npcLoot) {
			// Do NOT misuse the ModifyNPCLoot and OnKill hooks: the former is only used for registering drops, the latter for everything else

			// Add the treasure bag using ItemDropRule.BossBag (automatically checks for expert mode)
			//npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<MinionBossBag>()));

			// Trophies are spawned with 1/10 chance
            //removed this because it was shit

			// All our drops here are based on "not expert", meaning we use .OnSuccess() to add them into the rule, which then gets added
			//LeadingConditionRule notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());

			
			//notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<MinionBossMask>(), 7));

        }
		public override void FindFrame(int frameHeight) {
			// This NPC animates with a simple "go from start frame to final frame, and loop back to start frame" rule
			// In this case: First stage: 0-1-2-0-1-2, Second stage: 3-4-5-3-4-5, 5 being "total frame count - 1"
			int startFrame = 1;
			int finalFrame = 4;

			
			NPC.frameCounter += 0.5f;
			
			if (NPC.frameCounter > 2) {
				NPC.frameCounter = 0;
				NPC.frame.Y += frameHeight;

				if (NPC.frame.Y > finalFrame * frameHeight) {
					NPC.frame.Y = startFrame * frameHeight;
				    }
			    }
             }
        }
	}


//These first three are required 
//The fourth is calling for code in the projectiles folder
