include<stdio.h>

int main()
{
    

    int a,b,c,d; 
    
    
    printf("write first no.");
    scanf("%d",&a); 
    
    printf("write second no.");
    scanf("%d",&b);
    
    printf("which operation you want to do\n");
    printf("1:multiply\n");
    printf("2:Division\n");  
    printf("3:addition \n");
    printf("4:subtraction\n");  
    scanf("%d",&c);
    
    if(c==1){
    
    printf("%d\n", a*b);
    }
    
    else if(c==2){
    printf("%d\n",a/b);
    }
    
    else if(c==3){
    printf("%d\n",a+b);
    }
    else if(c==4){
    printf("%d\n",a-b);
    }
    
    else{
    printf("enter number between 1 and 4\n");
    }
    
