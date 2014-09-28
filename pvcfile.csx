pvc.Task("build20", () => {
  pvc.Source("Promises/Promises.csproj").Pipe(new PvcMSBuild( targetFrameworkVersion: "v2.0" ));
});

pvc.Task("build40", () => {
  pvc.Source("Promises/Promises.csproj").Pipe(new PvcMSBuild( targetFrameworkVersion: "v4.0" ));
});

pvc.Task("tests", () => {
  pvc.Source(@"Promises.Tests\bin\Debug\Promises.Tests.dll").Pipe(new PvcXunit());;
}).Requires("build");

pvc.Task("build").Requires("build20", "build40");
pvc.Task("default").Requires("build");
