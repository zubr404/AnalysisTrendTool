using AnalisesTrendTool.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace AnalisesTrendTool.ViewModels
{
    /*----------------------------------------------
     * !!! Свойства не переименовывать, они привязаны к App.config
     ----------------------------------------------*/

    public class SettingsScrinView : PropertyChangedBase
    {
        private readonly OpenDialogService openDialogService;
        public SettingsScrinView()
        {
            LoadSettings();
            openDialogService = new OpenDialogService();
        }

        #region Properties
        public Visibility Visibility
        {
            get { return visibility; }
            set
            {
                visibility = value;
                base.NotifyPropertyChanged();
            }
        }
        private Visibility visibility = Visibility.Collapsed;

        public int EmaPeriodSettings1
        {
            get { return emaPeriodSettings1; }
            set
            {
                if (value > 0)
                {
                    emaPeriodSettings1 = value;
                    base.NotifyPropertyChanged();
                }
            }
        }
        private int emaPeriodSettings1;

        public int EmaPeriodSettings2
        {
            get { return emaPeriodSettings2; }
            set
            {
                if (value > 0)
                {
                    emaPeriodSettings2 = value;
                    base.NotifyPropertyChanged();
                }
            }
        }
        private int emaPeriodSettings2;

        public int EmaPeriodSettings3
        {
            get { return emaPeriodSettings3; }
            set
            {
                if (value > 0)
                {
                    emaPeriodSettings3 = value;
                    base.NotifyPropertyChanged();
                }
            }
        }
        private int emaPeriodSettings3;

        public string PairsFileNameSettings
        {
            get { return pairsFileNameSettings; }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    pairsFileNameSettings = value;
                    base.NotifyPropertyChanged();
                }
            }
        }
        private string pairsFileNameSettings;

        public string PairsSaveFileNameSettings
        {
            get { return pairsSaveFileNameSettings; }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    pairsSaveFileNameSettings = value;
                    base.NotifyPropertyChanged();
                }
            }
        }
        private string pairsSaveFileNameSettings;

        public int NumberBarAnalysisSettings
        {
            get { return numberBarAnalysisSettings; }
            set
            {
                if (value > 0)
                {
                    numberBarAnalysisSettings = value;
                    base.NotifyPropertyChanged();
                }
            }
        }
        private int numberBarAnalysisSettings;
        #endregion

        #region Command
        /// <summary>
        /// Открытие диалога для получения пути к файлу с парами для получения данных
        /// </summary>
        public RelayCommand GetPairsFileCommand
        {
            get
            {
                return getPairsFileCommand ?? new RelayCommand((object o) =>
                {
                    try
                    {
                        PairsFileNameSettings = getFilesPath();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                });
            }
        }
        private RelayCommand getPairsFileCommand;
        /// <summary>
        /// Открытие диалога для получения пути к файлу, в который будут записаны пары с трендом
        /// </summary>
        public RelayCommand GetPairsFileSaveCommand
        {
            get
            {
                return getPairsFileSaveCommand ?? new RelayCommand((object o) =>
                {
                    try
                    {
                        PairsSaveFileNameSettings = getFilesPath();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                });
            }
        }
        private RelayCommand getPairsFileSaveCommand;
        public RelayCommand SaveCommand
        {
            get
            {
                return saveComand ?? new RelayCommand((object o) =>
                {
                    ConfigurationService.Update(nameof(EmaPeriodSettings1), EmaPeriodSettings1.ToString());
                    ConfigurationService.Update(nameof(EmaPeriodSettings2), EmaPeriodSettings2.ToString());
                    ConfigurationService.Update(nameof(EmaPeriodSettings3), EmaPeriodSettings3.ToString());
                    ConfigurationService.Update(nameof(PairsFileNameSettings), PairsFileNameSettings);
                    ConfigurationService.Update(nameof(PairsSaveFileNameSettings), PairsSaveFileNameSettings);
                    ConfigurationService.Update(nameof(NumberBarAnalysisSettings), NumberBarAnalysisSettings.ToString());
                    Visibility = Visibility.Collapsed;
                });
            }
        }
        private RelayCommand saveComand;

        public RelayCommand CancelCommand
        {
            get
            {
                return cancelCommand ?? new RelayCommand((object o) =>
                {
                    Visibility = Visibility.Collapsed;
                });
            }
        }
        private RelayCommand cancelCommand;
        #endregion

        public void LoadSettings()
        {
            try
            {
                EmaPeriodSettings1 = int.Parse(ConfigurationService.Get(nameof(EmaPeriodSettings1)));
                EmaPeriodSettings2 = int.Parse(ConfigurationService.Get(nameof(EmaPeriodSettings2)));
                EmaPeriodSettings3 = int.Parse(ConfigurationService.Get(nameof(EmaPeriodSettings3)));
                PairsFileNameSettings = ConfigurationService.Get(nameof(PairsFileNameSettings));
                PairsSaveFileNameSettings = ConfigurationService.Get(nameof(PairsSaveFileNameSettings));
                NumberBarAnalysisSettings = int.Parse(ConfigurationService.Get(nameof(NumberBarAnalysisSettings)));
            }
            catch (Exception)
            {
                throw;
            }
        }

        private string getFilesPath()
        {
            if (openDialogService.OpenFileDialog())
            {
                return openDialogService.FilePath;
            }
            return "";
        }
    }
}
