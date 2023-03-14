// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using Benchmarks;

#if DEBUG
BenchmarkRunner.Run<DictionaryLoaderBenchmarks>(new DebugInProcessConfig());
#else
BenchmarkRunner.Run<DictionaryLoaderBenchmarks>();
#endif
