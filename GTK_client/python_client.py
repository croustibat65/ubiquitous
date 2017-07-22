import sys
from gi.repository import Gtk, Gdk


def callBack(*args):
    print("Clipboard changed. New value = " + clip.wait_for_text())


clip = Gtk.Clipboard.get(Gdk.SELECTION_CLIPBOARD)
clip.connect('owner-change',callBack)
Gtk.main()
