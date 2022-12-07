class AdventFile
{
    public string name;
    public int size;

    public AdventFile(string name, int size)
    {
        this.name = name;
        this.size = size;
    }
}

class AdventDirectory
{
    public string name;
    public AdventDirectory? parent;
    public List<AdventDirectory> childDirectories;
    public List<AdventFile> files;

    public AdventDirectory(string name, AdventDirectory? parent)
    {
        this.name = name;
        this.parent = parent;
        this.childDirectories = new List<AdventDirectory>();
        this.files = new List<AdventFile>();
    }
}

public class AdventOfCode
{
    public static void Main(string[] args)
    {
        AdventDirectory root = ParseDirectory();

        int part1 = Part1(root);
        int part2 = Part2(root);

        Console.WriteLine($"Part One: {part1}.");
        Console.WriteLine($"Part Two: {part2}.");
    }

    static AdventDirectory ParseDirectory()
    {
        AdventDirectory? root = null;
        AdventDirectory? currentDir = null;

        using TextReader tr = File.OpenText("input_01.txt");
        
        bool lsEnabled = false;
        while (true)
        {
            string? line = tr.ReadLine();
            if (string.IsNullOrEmpty(line))
                break;
            
            string[] parts = line.Split(" ");

            // Bootstrap the root of the file system.
            if (root == null)
            {
                if (line != "$ cd /")
                {
                    throw new InvalidDataException();
                }

                root = new AdventDirectory("/", parent: null);
                currentDir = root;
                continue;
            }

            // Handle commands
            if (parts[0] == "$")
            {
                lsEnabled = false;

                if (parts[1] == "cd")
                {
                    if (parts[2] == "..")
                    {
                        currentDir = currentDir?.parent;
                    }
                    else if (parts[2] == "/")
                    {
                        currentDir = root;
                    }
                    else
                    {
                        foreach (AdventDirectory dir in currentDir!.childDirectories)
                        {
                            if (dir.name == parts[2])
                            {
                                currentDir = dir;
                            }
                        }
                    }
                    
                }
                else if (parts[1] == "ls")
                {
                    lsEnabled = true;
                }

                continue;
            }

            if (lsEnabled)
            {
                if (parts[0] == "dir")
                {
                    currentDir!.childDirectories.Add(new AdventDirectory(parts[1], currentDir));
                }
                else
                {
                    // File
                    currentDir!.files.Add(new AdventFile(parts[1], int.Parse(parts[0])));
                }
            }
        }

        return root!;
    }

    static int FindAllDirectorySizes(AdventDirectory current, List<int> sizesResult)
    {
        int childSizes = 0;
        foreach (AdventDirectory child in current.childDirectories)
        {
            childSizes += FindAllDirectorySizes(child, sizesResult);
        }

        foreach (AdventFile file in current.files)
        {
            childSizes += file.size;
        }

        sizesResult.Add(childSizes);
        
        return childSizes;
    }

    static int Part1(AdventDirectory root)
    {
        List<int> sizesResult = new List<int>();
        FindAllDirectorySizes(root, sizesResult);
        return sizesResult.Where((x) => x <= 100000).Sum();
    }

    static int Part2(AdventDirectory root)
    {
        List<int> sizesResult = new List<int>();
        int rootSize = FindAllDirectorySizes(root, sizesResult);
        int spaceAvailable = 70000000 - rootSize;
        int spaceNeeded = 30000000 - spaceAvailable;
        if (spaceNeeded < 0)
        {
            spaceNeeded = 0;
        }
        sizesResult.Sort();
        return sizesResult.Where(x => x >= spaceNeeded).First();
    }
}