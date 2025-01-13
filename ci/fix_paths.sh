echo "Finding coverage files in $1"

if [ -d "$1" ]; then
    find $1 -name "*.xml" -type f -print0 | xargs -0 -I {} sh -c '
        echo "Fixing paths in {}"
        sed -i "s/\/github\/workspace\//.\//g" "{}"
    '
fi