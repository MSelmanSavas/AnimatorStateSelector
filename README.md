# AnimationStateSelector
Easy access for animator state selection. For Unity 2021.3+!

This tool can be used for selecting animation state names in editor time to be used in runtime.
This can be used for basic string names for selecting animator states, or it can be used with its custom AnimatorStateData class for more info and easy access to animator state info.

## String usage without Animator variable

If declared like shown in the images, it automatically picks the attached animator for selecting its animation states.

![Unity_j01hYkbIR9](https://user-images.githubusercontent.com/105663238/210182679-5d96976e-063d-40db-9878-c6611c8c0d4a.png)

![Unity_RqCD9mBs5S](https://user-images.githubusercontent.com/105663238/210182680-ca73ae6e-9950-4c1d-b74f-e590bed0f8b3.png)

![Code_DgUsZwBvud](https://user-images.githubusercontent.com/105663238/210182681-43f4e92f-d1ac-4ad2-98c7-7fd9690c6322.png)

## String usage with Animator variable

If declared like show in the images, it can find the name of the animator and use the assigned value for the animator field. If there is nothing attached, it will grab the animator on the gameObject it is attached to.

![Unity_PcnVwKXj6h](https://user-images.githubusercontent.com/105663238/210182718-67eee24e-40a0-4949-9254-012d5233ba0f.png)

![Unity_LXB69QeXgT](https://user-images.githubusercontent.com/105663238/210182721-d06cbcd4-f67c-4601-bed0-4fc1007f28be.png)

![Unity_0fPQY359lF](https://user-images.githubusercontent.com/105663238/210182722-61e175d0-85bb-40c6-83e2-b82ee7a4b665.png)

![Code_I7ynixDys8](https://user-images.githubusercontent.com/105663238/210182725-c1ab621b-818b-4b9c-8ce6-8ec8e4485d1e.png)

## AnimatorStateData usage without Animator variable

Using AnimatorStateData class, it can grab more data about selected state, such as it's Hash for more optimized animator play usage, such as using Animator.Play(AnimatorStateData.Hash), name of the selected state, clip of the selected state and length of the selected state.

![image](https://user-images.githubusercontent.com/105663238/210182834-171fa699-f348-432a-a183-ae5f241c9da1.png)
![image](https://user-images.githubusercontent.com/105663238/210182860-9436b20a-c223-4eff-b836-8d968bfc1ea8.png)

## AnimatorStateData usage with given Animator variable

If there is no Animator given, it will automatically grab the animator on the gameObject it gets attached to. With given Animator, it will collect data from the given animator.

![image](https://user-images.githubusercontent.com/105663238/210183031-9f1b6763-b34f-4398-af9f-67f7845b7cf2.png)

## String and AnimatorStateData List Usage

All of the former explained features can be used in list form too!

![Unity_3SRISbsfUB](https://user-images.githubusercontent.com/105663238/210183404-9bb20791-31bc-490b-b1dc-a068af362c58.png)
![Code_VdWecUblUL](https://user-images.githubusercontent.com/105663238/210183409-84845e10-c7ec-4833-8c44-fce594616339.png)


## Basic Examples of AnimatorStateData usage:

![Code_pjhS0ofVmN](https://user-images.githubusercontent.com/105663238/210182960-8b5c1d08-1f70-4149-b994-262b33837f0c.png)
![Code_FicyK5AriD](https://user-images.githubusercontent.com/105663238/210182961-62fe2979-64f2-4bbe-b699-16e67f78b35e.png)

