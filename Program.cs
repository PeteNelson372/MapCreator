using Application = System.Windows.Forms.Application;

namespace MapCreator
{
    internal static class Program
    {
        public static readonly log4net.ILog LOGGER = log4net.LogManager.GetLogger(typeof(MainForm));

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]

        static void Main()
        {
            log4net.Config.XmlConfigurator.Configure();

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());
        }
    }
}