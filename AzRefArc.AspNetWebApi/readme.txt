
・CORS を設定
　WASM アプリを配布するサイトと Web API を提供するサイトを別にしているため、CORS を設定
　オリジンについてはハードコーディングしているが、appsettings.json に切り出してもよい

・.NET 8 の Breaking Changes に伴う Workaround として以下の InvariantGlobalization を設定
  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
    <InvariantGlobalization>false</InvariantGlobalization>
    <UserSecretsId>9e0fb557-c77a-4e0a-b65c-c87f649a2620</UserSecretsId>
  </PropertyGroup>
