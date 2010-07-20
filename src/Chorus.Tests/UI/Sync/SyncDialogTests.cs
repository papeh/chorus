﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Chorus.sync;
using Chorus.UI.Sync;
using Chorus.VcsDrivers;
using LibChorus.Tests;
using NUnit.Framework;

namespace Chorus.Tests.UI.Sync
{
	[TestFixture]
	public class SyncDialogTests
	{
		[Test, Ignore("Run by hand only")]
		public void ShowSyncStartControl_NoPaths()
		{
			var setup = new RepositorySetup("pedro");
			{
				var c = new SyncStartControl(setup.Repository);
				var f = new Form();
				c.Dock = DockStyle.Fill;
				f.Controls.Add(c);
				Application.Run(f);
			}
		}

		[Test, Ignore("Run by hand only")]
		public void ShowSyncStartControl_InternetAndNetworkPaths()
		{
			var setup = new RepositorySetup("pedro");
			{
				setup.Repository.SetKnownRepositoryAddresses(new RepositoryAddress[]
																 {
																	 RepositoryAddress.Create("language depot", "http://hg-public.languagedepot.org"),
																	 RepositoryAddress.Create("joe's mac", "//suzie-pc/shared")
																 });
				var c = new SyncStartControl(setup.Repository);
				var f = new Form();
				c.Dock = DockStyle.Fill;
				f.Controls.Add(c);
				Application.Run(f);
			}
		}

		[Test, Ignore("Run by hand only")]
		public void LaunchDialog_ExampleForBob()
		{
			var setup = new RepositorySetup("pedro");
			{
				Application.EnableVisualStyles();

				setup.Repository.SetKnownRepositoryAddresses(new RepositoryAddress[]
																 {
																	 RepositoryAddress.Create("language depot", "http://pedro:mypassword@hg-public.languagedepot.org"),
																 });
				setup.Repository.SetDefaultSyncRepositoryAliases(new[] {"language depot"});

				using (var dlg = new SyncDialog(setup.ProjectFolderConfig,
												SyncUIDialogBehaviors.StartImmediatelyAndCloseWhenFinished,
												SyncUIFeatures.Minimal))
				{
					dlg.ShowDialog();
				}
			}
		}

		[Test, Ignore("Run by hand only")]
		public void LaunchDialog_GoodForCancelTesting()
		{
			var setup = new RepositorySetup("pedro");
			{
				Application.EnableVisualStyles();

				setup.Repository.SetKnownRepositoryAddresses(new RepositoryAddress[]
																 {
																	 RepositoryAddress.Create("language depot", "http://automatedtest:testing@hg-public.languagedepot.org/tpi"),
																 });
				setup.Repository.SetDefaultSyncRepositoryAliases(new[] { "language depot" });

				using (var dlg = new SyncDialog(setup.ProjectFolderConfig,
												SyncUIDialogBehaviors.StartImmediately,
												SyncUIFeatures.NormalRecommended))
				{
					dlg.ShowDialog();
				}
			}
		}

		[Test, Ignore("Run by hand only")]
		public void LaunchDialog_LazyWithNormalUI()
		{
			var setup = new RepositorySetup("pedro");
			{
				Application.EnableVisualStyles();

				using (var dlg = new SyncDialog(setup.ProjectFolderConfig,
												SyncUIDialogBehaviors.Lazy,
												SyncUIFeatures.NormalRecommended))
				{
					dlg.ShowDialog();
				}

			}
		}

		[Test, Ignore("Run by hand only")]
		public void MinimalCodeToLaunchSendReceiveUI()
		{
			var projectConfig = new ProjectFolderConfiguration("c:\\TokPisin");
			projectConfig.IncludePatterns.Add("*.lift");

			using (var dlg = new SyncDialog(projectConfig,
											SyncUIDialogBehaviors.Lazy,
											SyncUIFeatures.NormalRecommended))
			{
				dlg.ShowDialog();
			}
		}

		[Test, Ignore("Run by hand only")]
		public void LaunchDialog_LazyWithAdvancedUI()
		{
			var setup = new RepositorySetup("pedro");
			{
				Application.EnableVisualStyles();

				using (var dlg = new SyncDialog(setup.ProjectFolderConfig,
												SyncUIDialogBehaviors.Lazy,
												SyncUIFeatures.Advanced))
				{
					//    dlg.SyncOptions.RepositorySourcesToTry.Add(RepositoryAddress.Create("bogus", @"z:/"));
					dlg.ShowDialog();
				}

			}
		}

		[Test, Ignore("Run by hand only")]
		public void LaunchDialog_MinimalUI()
		{
			var setup = new RepositorySetup("pedro");
			{
				Application.EnableVisualStyles();
				var dlg = new SyncDialog(setup.ProjectFolderConfig,
										 SyncUIDialogBehaviors.StartImmediately,
										 SyncUIFeatures.Minimal);

				dlg.ShowDialog();
			}
		}

		[Test, Ignore("By Hand Only (should be fine, but started causing problems on TeamCity")]
		public void LaunchDialog_AutoWithMinimalUI()
		{
			var setup = new RepositorySetup("pedro");
			{
				Application.EnableVisualStyles();
				var dlg = new SyncDialog(setup.ProjectFolderConfig,
										 SyncUIDialogBehaviors.StartImmediatelyAndCloseWhenFinished,
										 SyncUIFeatures.Minimal);

				dlg.ShowDialog();
			}
		}

		[Test, Ignore("By Hand Only")]
		public void LaunchDialog_BogusTarget_AdmitsError()
		{
			var setup = new RepositorySetup("pedro");
			{
				Application.EnableVisualStyles();
				using (var dlg = new SyncDialog(setup.ProjectFolderConfig,
												SyncUIDialogBehaviors.StartImmediatelyAndCloseWhenFinished,
												SyncUIFeatures.Minimal))
				{
					dlg.SyncOptions.RepositorySourcesToTry.Add(RepositoryAddress.Create("bogus", @"z:/"));
					dlg.ShowDialog();
					Assert.IsTrue(dlg.FinalStatus.WarningEncountered);
				}
			}
		}
	}
}