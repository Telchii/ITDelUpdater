using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITDelUp.Views
{
	public class ShellViewModel : Conductor<IScreen>.Collection.OneActive
	{
		public ShellViewModel()
		{
			this.DisplayName = "IT Delete Updater";
			ShowMainView();
		}

		public void ShowMainView()
		{
			//var mainView = IoC.Get<MainViewModel>();
			//ActivateItem(mainView);
			this.ActivateItem(new MainViewModel());
		}
	}
}