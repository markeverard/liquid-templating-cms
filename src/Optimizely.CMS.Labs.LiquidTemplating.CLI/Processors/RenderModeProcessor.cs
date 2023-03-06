using Fluid;
using Fluid.ViewEngine;
using Microsoft.Extensions.FileProviders;
using Optimizely.CMS.Labs.LiquidTemplating.CLI.Reader;
using Optimizely.CMS.Labs.LiquidTemplating.CLI.Writer;
using System;

namespace Optimizely.CMS.Labs.LiquidTemplating.CLI.Processors
{
    /// <summary>
    /// Watches for changes in given directory and pushs all changes to remote.
    /// </summary>
    public class RenderModeProcessor
    {
        private readonly string _path;
        private readonly string _file;
        private readonly IFileProvider _fileProvider;
     
        public RenderModeProcessor(string path, string file)
        {
            _path = path;
            _file = file;
            _fileProvider = new PhysicalFileProvider(path);
           }

        public async Task Process()
        {
            var parser = new FluidParser();

            var model = "Bill";
         
           
            var options = new FluidViewEngineOptions();
            options.TemplateOptions.FileProvider = _fileProvider;
            options.Parser = new FluidViewParser(new FluidParserOptions { AllowFunctions = true });
            options.ViewsFileProvider = _fileProvider;
            options.PartialsFileProvider = _fileProvider;
            options.TrackFileChanges = false;

            var context = new TemplateContext(model, new TemplateOptions() { FileProvider = _fileProvider, MemberAccessStrategy = new UnsafeMemberAccessStrategy() });
            var renderer = new FluidViewRenderer(options);

            //using (var writer = new StringWriter())
            //{
            //    await renderer.RenderViewAsync(writer, "startpage\\index.liquid", context);
            //    await writer.FlushAsync();
            //    Console.WriteLine(writer.ToString());

            //}

            var fileName = _file.Replace('\\', '-').Replace(".liquid", ".html");



            using (StreamWriter writer = new StreamWriter(Path.Combine(_path, fileName)))
            {
                await renderer.RenderViewAsync(writer, _file, context);
                await writer.FlushAsync();
                //writer.WriteLine(writer.ToString());
            }




                //open a website


                Console.WriteLine("Press enter to exit.");
            Console.ReadLine();
        }

       
    }
}


