using System;
using System.Diagnostics;
using Microsoft.VisualBasic.FileIO;

namespace TrabalhoSort
{
    class Program
    {
        // Contadores
        private static int compara = 0, move = 0;

        static void Main(string[] args)
        {
            // Para identificar o arquivo e o algoritmo de ordenação usado
            string caminho, algoritmo = "";
            // Para marcar o tempo de execução do código
            Stopwatch stopwatch = new Stopwatch();
            
            // Executa o código enquanto o usuário tiver algo para ordenar
            while (true)
            {
                // Impede que o usuario forneça um arquivo que não exista
                while (true)
                {
                    Console.WriteLine();

                    Console.WriteLine("Entre com o caminho para o arquivo (.csv) que contem os dados a serem ordenados:");
                    caminho = Console.ReadLine();

                    Console.WriteLine();

                    if (FileSystem.FileExists(caminho))
                        break;
                    else
                        Console.WriteLine(caminho + " não foi encontrado, digite um caminho existente");
                }

                // Pede o código do método de ordenação
                Console.WriteLine("Qual será o metodo de ordenação: (Q: Quick, M: Merge, I: Insertion, S: Selection)");
                char metodo = char.ToUpper(Console.ReadLine()[0]);

                // Pega o arquivo que contem os dados
                TextFieldParser data = new TextFieldParser(caminho);
                string cabecalho = data.ReadLine();

                string[] ordenar = new string[5000];

                int tamanho = 0;
                
                // Copia os dados do arquivo para um vetor e contabiliza o tamanho
                while (!data.EndOfData)
                {
                    ordenar[tamanho] = data.ReadLine();
                    tamanho++;
                }

                // Verifica qual o método escolhido e chama o algoritmo de ordenação adequado
                if (metodo == 'I')
                {
                    algoritmo = "Insertion Sort";
                    stopwatch.Restart();

                    ordenar = InsertionSort(ordenar, tamanho);

                    stopwatch.Stop();

                }
                else if (metodo == 'S')
                {
                    algoritmo = "Selection Sort";
                    stopwatch.Restart();

                    ordenar = SelectionSort(ordenar, tamanho);

                    stopwatch.Stop();

                }
                else if (metodo == 'M')
                {
                    algoritmo = "Merge Sort";
                    stopwatch.Restart();

                    ordenar = MergeSort(ordenar, 0, tamanho - 1);

                    stopwatch.Stop();

                }
                else if (metodo == 'Q')
                {
                    algoritmo = "Quick Sort";
                    stopwatch.Restart();

                    ordenar = Quicksort(ordenar, 0, tamanho - 1);

                    stopwatch.Stop();
                }
                
                // Mostra na tela os dados ordenados, o tempo decorrido, as comparações e movimentações
                Console.WriteLine();
                Console.WriteLine(cabecalho);

                for (int i = 0; i < tamanho; i++)
                {
                    Console.WriteLine(ordenar[i]);
                }

                Console.WriteLine();

                Console.WriteLine("Tempo decorrido (em milisegundos) para a execução do " + algoritmo + " para " + tamanho + " dados: " + stopwatch.ElapsedMilliseconds);
                Console.WriteLine("Número de comparações: " + compara);
                Console.WriteLine("Número de movimentações: " + move);
                
                // Reinicia o número de comparações e movimentações
                compara = 0; move = 0;

                // Verifica se o usuário deseja realizar outra ordenação
                Console.WriteLine();
                Console.WriteLine("Deseja realizar outra ordenação? (S/N)");
                
                // Se a resposta for sim, limpa o prompt e recomeça o while
                if (char.ToUpper(Console.ReadLine()[0]) == 'N')
                    break;
                else
                    Console.Clear();
                
                // fecha o arquivo
                data.Close();
            }
        }

        private static string[] InsertionSort(string[] vec, int tam)
        {
            string chave = "";
            int chavePos = 0;

            for (int i = 0; i < tam - 1; i++)
            {
                chavePos = i + 1;
                chave = vec[chavePos];

                for (int j = i; j > -1; j--)
                {
                    // comparação
                    compara++;
                    if (String.Compare(chave.Split(',')[0], vec[j].Split(',')[0], true) == -1)
                    {
                        // movimentação
                        vec[chavePos] = vec[j]; move++;
                        chavePos = j;
                    }
                }
                // movimentação
                vec[chavePos] = chave; move++;
            }

            // vetor ordenado
            return vec;
        }

        private static string[] SelectionSort(string[] vec, int tam)
        {
            int menorPos = 0;
            string aux = "";

            for (int i = 0; i < tam; i++)
            {
                menorPos = i;
                for (int j = i; j < tam; j++)
                {
                    // comparação
                    compara++;
                    if (String.Compare(vec[menorPos].Split(',')[0], vec[j].Split(',')[0], true) == 1)
                    {
                        menorPos = j;
                    }
                }

                // comparação
                compara++;
                if(vec[i] != vec[menorPos])
                {
                    // movimentação
                    aux = vec[i];
                    vec[i] = vec[menorPos];
                    vec[menorPos] = aux; move++;
                }                
            }

            // vetor ordenado
            return vec;
        }

        private static string[] MergeSort(string[] vec, int firstPos, int lastPos)
        {
            if (lastPos > firstPos)
            {
                int dividiPos = (firstPos + lastPos) / 2;
                MergeSort(vec, firstPos, dividiPos);
                MergeSort(vec, dividiPos + 1, lastPos);
                Combina(vec, firstPos, dividiPos, lastPos);
            }

            // vetor ordenado
            return vec;
        }

        private static string[] Quicksort(string[] vec, int firstPos, int lastPos)
        {
            if (firstPos < lastPos)
            {
                int pivo = Particiona(vec, firstPos, lastPos);
                Quicksort(vec, firstPos, pivo - 1);
                Quicksort(vec, pivo + 1, lastPos);
            }

            // vetor ordenado
            return vec;
        }

        private static void Combina(string[] vec, int firstPos, int halfPos, int lastPos)
        {
            string[] primeira = new string[halfPos - firstPos + 1];
            string[] ultima = new string[lastPos - halfPos];

            for (int i = 0; i < primeira.Length; i++)
            {
                primeira[i] = vec[firstPos + i];
            }

            for (int i = 0; i < ultima.Length; i++)
            {
                ultima[i] = vec[halfPos + 1 + i];
            }

            int priPos = 0, ultPos = 0, vecPos = firstPos;

            while (priPos < primeira.Length && ultPos < ultima.Length)
            {
                // comparação
                compara++;
                if (String.Compare(primeira[priPos].Split(',')[0], ultima[ultPos].Split(',')[0], true) == -1)
                {
                    // movimentação
                    vec[vecPos] = primeira[priPos];
                    priPos++;
                }
                else
                {
                    // movimentação
                    vec[vecPos] = ultima[ultPos];
                    ultPos++;
                }                               
                vecPos++; move++;
            }

            while (priPos < primeira.Length)
            {
                // movimentação
                vec[vecPos] = primeira[priPos]; move++;
                priPos++; vecPos++;
            }

            while (ultPos < ultima.Length)
            {
                // movimentação
                vec[vecPos] = ultima[ultPos]; move++;
                ultPos++; vecPos++;
            }
        }

        private static int Particiona(string[] vec, int comeco, int fim)
        {
            int p = comeco + 1;
            string pivo = vec[comeco];
            string aux;

            for (int i = comeco + 1; i <= fim; i++)
            {
                // comparação
                compara++;
                if (String.Compare(vec[i].Split(',')[0], pivo.Split(',')[0]) == -1)
                {
                    // movimentação
                    aux = vec[i];
                    vec[i] = vec[p];
                    vec[p] = aux; move++;
                    p++;
                }
                
            }
            // movimentação
            vec[comeco] = vec[p - 1];
            vec[p - 1] = pivo; move++;

            // posição do pivo
            return p - 1;
        }
    }
}
