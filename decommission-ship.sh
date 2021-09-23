#!/bin/bash

echo "NOTE: Ships should be decommissioned in reverse alphabetical order."
echo "ALSO: Are you really sure you want to do this?"
exit

for ship in $*
do
    echo "Decommissioning: $ship ..."
    # comment out lines that mention the ship
    sed -i -e "s/\(.*$ship.*\)/<!-- \1 -->/" \
        'Spaceship Collaboration Activity.csproj' 
    sed -i -e "s/\(.*$ship.*\)/\/\/\1/" \
        Sandbox/Scripts/ShipNames.cs
    git add \
        'Spaceship Collaboration Activity.csproj' \
        Sandbox/Scripts/ShipNames.cs

    # zip up files for the ship
    zip -r decommissioned-ships.zip \
        SubsystemControllers/$ship \
        Sandbox/Scenes/Ships/$ship*

    # stage the removals
    git rm -r \
        SubsystemControllers/$ship \
        Sandbox/Scenes/Ships/$ship*

done

