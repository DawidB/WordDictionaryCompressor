# Introduction

The idea behind the project was to create a library that can be used to compress and decompress word dictionary files used in Scrabble games. The library was supposed to be faster than decompression of ZIP file, at the expense of worse compression ratio - basically a middle ground between storing data in plaintext (light on CPU, heavy on storage) and ZIP file (heavy on CPU, light on storage).

# Results

The results are unfortunately disappointing, as shown on benchmark results below. The library is both slower than ZIP decompression (see LoadZipLines vs LoadCwdLines) and yields worse compression ratio (x2 on English dictionary compared to ZIP). The library is also slower than reading the file line by line, but the same can be said of ZIP compression obviously. 

Sample benchmark run on machine equipped with M2 SSD:
|                Method |       Mean |     Error |    StdDev | Ratio | Allocated | Alloc Ratio |
|---------------------- |-----------:|----------:|----------:|------:|----------:|------------:|
|             LoadLines |  31.696 ms | 0.5430 ms | 0.5079 ms |  1.00 |  21.65 MB |        1.00 |
|          LoadZipLines |  37.100 ms | 0.7009 ms | 0.5853 ms |  1.17 |  13.66 MB |        0.63 |
|          LoadCwdLines | 213.536 ms | 4.2634 ms | 5.0753 ms |  6.76 | 188.32 MB |        8.70 |
| LoadOptimizedCwdLines | 108.290 ms | 2.0859 ms | 2.5617 ms |  3.44 |     39 MB |        1.80 |

The attempt to optimize it improved results considerably (see LoadCwdLines vs LoadOptimizedCwdLines), but still the library is slower than reading the ZIP file.  

There is certainly room for improvement, but there is small possibility that the library will be faster than ZIP anyway, so I can only publish it as an interesting experiment.