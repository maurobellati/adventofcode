namespace AdventOfCode.Tool.Infrastructure;

using Spectre.Console.Cli;

public sealed class TypeResolver(IServiceProvider provider) : ITypeResolver, IDisposable
{
    public void Dispose()
    {
        if (provider is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }

    public object Resolve(Type? type)
    {
        ArgumentNullException.ThrowIfNull(type);
        return provider.GetService(type) ?? throw new InvalidOperationException($"Could not resolve type '{type.FullName}'.");
    }
}
