using Avalonia;
using Avalonia.Logging.Serilog;
using Avalonia.Markup.Xaml;
using AvalonStudio.Packages;
using AvalonStudio.Platforms;
using AvalonStudio.Repositories;
using Serilog;
using System;

namespace AvalonConsole
{
  internal class App : Application
    {
        static void Print(Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.StackTrace);
            if (ex.InnerException != null)
            {
                Print(ex.InnerException);
            }
        }

        [STAThread]
        private static void Main(string[] args)
        {
            try
            {
                if (args == null)
                {
                    throw new ArgumentNullException(nameof(args));
                }

                var builder = BuildAvaloniaApp().AfterSetup(async _ =>
                {
                    Platform.Initialise();
                    PackageSources.InitialisePackageSources();

                    //var extensionManager = new ExtensionManager();
                    //var container = CompositionRoot.CreateContainer(extensionManager);

                    //var shellExportFactory = container.GetExport<ExportFactory<ShellViewModel>>();
                    //ShellViewModel.Instance = shellExportFactory.CreateExport().Value;

                    await PackageManager.LoadAssetsAsync().ConfigureAwait(false);
                });

                InitializeLogging();

                builder.Start<MainWindow>();                
            }
            catch (Exception e)
            {
                Print(e);
            }
            finally
            {
                Application.Current.Exit();
            }
        }

        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>().UsePlatformDetect().UseSkia().UseReactiveUI();

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private static void InitializeLogging()
        {
#if DEBUG
            SerilogLogger.Initialize(new LoggerConfiguration()
                .MinimumLevel.Warning()
                .WriteTo.Trace(outputTemplate: "{Area}: {Message}")
                .CreateLogger());
#endif
        }
    }
}