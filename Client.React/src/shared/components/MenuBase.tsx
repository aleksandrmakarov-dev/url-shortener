import { DialogTrigger } from "@radix-ui/react-dialog";
import { useMediaQuery } from "../hooks/useMediaQuery";
import { Button } from "../ui/button";
import {
  DrawerTrigger,
  DrawerContent,
  DrawerFooter,
  DrawerClose,
  Drawer,
  DrawerHeader,
  DrawerTitle,
} from "../ui/drawer";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuTrigger,
} from "../ui/dropdown-menu";
import { useState } from "react";
import React from "react";

interface MenuBaseProps {
  trigger: JSX.Element;
  label?: React.ReactNode;
  children: React.ReactNode;
}

export function MenuBase(props: MenuBaseProps) {
  const { trigger, label, children } = props;
  const isDesktop = useMediaQuery("(min-width: 640px)");
  const [open, setOpen] = useState<boolean>();

  if (isDesktop) {
    return (
      <DropdownMenu open={open} onOpenChange={setOpen}>
        <DropdownMenuTrigger asChild>{trigger}</DropdownMenuTrigger>
        <DropdownMenuContent className="w-full max-w-96 absolute top-0 -right-4">
          {label && (
            <DropdownMenuLabel className="truncate p-2">
              {label}
            </DropdownMenuLabel>
          )}
          {React.Children.map(children, (child) => (
            <DropdownMenuItem>{child}</DropdownMenuItem>
          ))}
        </DropdownMenuContent>
      </DropdownMenu>
    );
  }

  return (
    <Drawer open={open} onOpenChange={setOpen}>
      <DrawerTrigger asChild>
        {trigger && <DialogTrigger asChild>{trigger}</DialogTrigger>}
      </DrawerTrigger>
      <DrawerContent>
        <DrawerHeader className="text-left">
          <DrawerTitle>{label}</DrawerTitle>
        </DrawerHeader>
        <div className="p-5 pb-2 space-y-2">
          {React.Children.map(children, (child) => (
            <Button
              onClick={() => setOpen(false)}
              asChild
              className="w-full"
              variant="outline"
            >
              {child}
            </Button>
          ))}
        </div>
        <DrawerFooter className="pt-2">
          <DrawerClose asChild>
            <Button variant="outline">Cancel</Button>
          </DrawerClose>
        </DrawerFooter>
      </DrawerContent>
    </Drawer>
  );
}
