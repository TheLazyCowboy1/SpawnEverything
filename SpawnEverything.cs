using System;
using System.Security;
using System.Security.Permissions;
using BepInEx;
using MonoMod.Cil;

#pragma warning disable CS0618

[module: UnverifiableCode]
[assembly: SecurityPermission(System.Security.Permissions.SecurityAction.RequestMinimum, SkipVerification = true)]

namespace SpawnEverything;

[BepInPlugin("LazyCowboy.SpawnEverything", "Spawn Everything", "0.0.1")]
public partial class SpawnEverything : BaseUnityPlugin
{

    //public static RegionRandomizerOptions Options;

    public static SpawnEverything Instance;

    public SpawnEverything()
    {
        try
        {
            Instance = this;
            //Options = new RegionRandomizerOptions(this, Logger);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex);
            throw;
        }
    }

    private void OnEnable()
    {
        On.RainWorld.OnModsInit += RainWorld_OnModsInit;
        //RegionLoader.Enable();
    }

    private void OnDisable()
    {
        //RegionLoader.Disable();
        if (IsInit)
        {
            IL.WorldLoader.ctor_RainWorldGame_Name_bool_string_Region_SetupValues -= WorldLoaderHook;
        }
    }

    //private static RainWorldGame game;


    private bool IsInit;
    private void RainWorld_OnModsInit(On.RainWorld.orig_OnModsInit orig, RainWorld self)
    {
        orig(self);
        try
        {
            if (IsInit) return;

            //Your hooks go here

            IL.WorldLoader.ctor_RainWorldGame_Name_bool_string_Region_SetupValues += WorldLoaderHook;

            //MachineConnector.SetRegisteredOI("LazyCowboy.KarmaExpansion", Options);
            IsInit = true;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex);
            throw;
        }
    }

    #region HOOKS
    public static void WorldLoaderHook(ILContext il)
    {
        try
        {
            ILCursor c = new ILCursor(il);

            if (c.TryGotoNext(MoveType.After,
                x => x.MatchAdd(),
                x => x.MatchCallvirt<String>("Substring"),
                x => x.MatchStelemRef(),
                x => x.MatchLdloc(8)
            ))
            {
                Instance.Logger.LogDebug("Found hook location; adding hook");
                //c.Index--;
                //c.Remove();
                //c.Remove();
                //c.Index += 3;
                //c.RemoveRange(2); //just remove the branch? (also must remove loading the unused variable to the stack)
                //c.Index += 3;
                //c.Emit(Mono.Cecil.Cil.OpCodes.Stloc_S, (byte)8);
                //c.Emit(Mono.Cecil.Cil.OpCodes.Ldc_I4_1, (int)1);
                //c.Remove();
                //c.Emit(Mono.Cecil.Cil.OpCodes.Ldc_I4_1, (int)1);
                c.Emit(Mono.Cecil.Cil.OpCodes.Pop);
                c.Emit(Mono.Cecil.Cil.OpCodes.Ldc_I4, (int)1);
                //c.Emit(Mono.Cecil.Cil.OpCodes.Ldarg_0);
                //c.EmitDelegate<Func<WorldLoader, bool>>((loader) => true);
            }
            
            /*
            if (c.TryGotoNext(MoveType.Before,
                //x => x.Match(Mono.Cecil.Cil.OpCodes.Ldc_I4_0),
                x => x.MatchLd
                x => x.MatchStloc(8)
                ))
            {
                c.Next.Operand = 1;
                //c.Remove();
                //c.Emit(Mono.Cecil.Cil.OpCodes.Ldc_I4_1, 1);
            }
            */
        } catch (Exception ex)
        {
            Instance.Logger.LogError(ex);
        }
    }
    #endregion
}