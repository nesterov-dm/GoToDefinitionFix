using System;
using System.Runtime.InteropServices;
using EnvDTE;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;

namespace GoToDefinitionFix
{
	[PackageRegistration(UseManagedResourcesOnly = true)]
	[InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
	[Guid("1DEFB92F-1CCA-497D-B7A3-2AA8938188F3")]
	[ProvideAutoLoad(VSConstants.UICONTEXT.SolutionExists_string)]
	public sealed class GoToDefinitionFixPackage : Package
	{
		private static CommandEvents _CommandEvents = null;
		private static Property _PropertyInsertTabs = null;
		private static bool _PropertyModified = false;


		protected override void Initialize()
		{
			base.Initialize();

			DTE dte = (DTE)GetService(typeof(DTE));

			_CommandEvents = dte.Events.CommandEvents;
			_PropertyInsertTabs = dte.Properties["TextEditor", "CSharp"].Item("InsertTabs");

			_CommandEvents.BeforeExecute += Command_BeforeExecute;
			_CommandEvents.AfterExecute += Command_AfterExecute;
		}


		private static void Command_BeforeExecute(string guid, int id, object customIn, object customOut, ref bool cancelDefault)
		{
			if(IsGoToDefinitionCommand(guid, id) && Equals(_PropertyInsertTabs.Value, true))
			{
				_PropertyInsertTabs.Value = false;
				_PropertyModified = true;
			}
		}

		private static void Command_AfterExecute(string guid, int id, object customIn, object customOut)
		{
			if(_PropertyModified && IsGoToDefinitionCommand(guid, id))
			{
				_PropertyInsertTabs.Value = true;
				_PropertyModified = false;
			}
		}


		private static bool IsGoToDefinitionCommand(string guid, int id)
		{
			return guid == VSConstants.CMDSETID.StandardCommandSet97_string && id == (int)VSConstants.VSStd97CmdID.GotoDefn;
		}
	}
}
