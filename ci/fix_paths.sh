if [ -d "Coverage/unity" ]; then
    find Coverage/unity -name "*.xml" -type f -print0 | xargs -0 -I {} sh -c '
        echo "Fixing paths in {}"
        sed -i "s/\/github\/workspace\//.\//g" "{}"
    '
fi

if [ -d "Coverage/generator" ]; then
    find Coverage/generator -name "*.xml" -type f -print0 | xargs -0 -I {} sh -c '
        echo "Fixing .NET paths in {}"
        sed -i "s/\/home\/runner\/work\/scriptable-values\/scriptable-values\///g" "{}"
    '
fi