set /p comment="Comment: "
git add --a
git commit -m "%comment%"
git push origin uidev