git pull origin uidev
git add --a
git commit -m "autocommit"
git push origin uidev
git branch -f uitest uidev
git push origin uitest
git branch -f uitest uiwork
git push origin uiwork