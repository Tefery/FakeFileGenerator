// Nombre del archivo
string nombreArchivo = args[0];

// Tamaño total en bytes (30 GB)+
long tamanioInMb = long.Parse(args[1]);
long tamañoTotal = tamanioInMb * 1024 * 1024;

if (tamanioInMb > 5000)
{
    Console.WriteLine($"ESTÁ A PUNTO DE GENERAR UN ARCHIVO DE {tamanioInMb}MB ¿ESTÁ SEGURO? y/n...");
    string respuesta = Console.ReadLine();

    if(!string.Equals("y", respuesta, StringComparison.InvariantCultureIgnoreCase))
    {
        Console.WriteLine("No se que has puesto, pero no es una Y, así que por si acaso, no seguimos...");
        return;
    }
}

// Tamaño del buffer (por ejemplo, 200 MB)
int tamañoBuffer = 200 * 1024 * 1024;

// Número total de iteraciones
long iteraciones = tamañoTotal / tamañoBuffer;

// Resto si el tamaño total no es múltiplo del tamaño del buffer
int resto = (int)(tamañoTotal % tamañoBuffer);

Console.WriteLine($"Generando archivo de {tamañoTotal} bytes (~{args[1]} MB)...");

// Usar FileStream con acceso asíncrono
using (FileStream fs = new FileStream(Path.Combine(Environment.CurrentDirectory, nombreArchivo), FileMode.Create, FileAccess.Write, FileShare.None, bufferSize: tamañoBuffer, useAsync: true))
{
    byte[] buffer = new byte[tamañoBuffer];
    Random random = new Random();

    for (long i = 0; i < iteraciones; i++)
    {
        random.NextBytes(buffer);
        await fs.WriteAsync(buffer, 0, buffer.Length);

        if (i % 100 == 0)
        {
            Console.WriteLine($"Escrito {i * tamañoBuffer / (1024 * 1024)} MB...");
        }
    }

    if (resto > 0)
    {
        byte[] bufferFinal = new byte[resto];
        random.NextBytes(bufferFinal);
        await fs.WriteAsync(bufferFinal, 0, bufferFinal.Length);
    }
}

Console.WriteLine("Archivo generado exitosamente.");